--
-- PostgreSQL database dump
--

-- Dumped from database version 15.4
-- Dumped by pg_dump version 15.4

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: uuid-ossp; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;


--
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


--
-- Name: bet_status; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.bet_status AS ENUM (
    'win',
    'lost',
    'in_progress'
);


ALTER TYPE public.bet_status OWNER TO postgres;

--
-- Name: coefficient_type; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.coefficient_type AS ENUM (
    'win_team1',
    'win_team2',
    'draw'
);


ALTER TYPE public.coefficient_type OWNER TO postgres;

--
-- Name: event_status; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.event_status AS ENUM (
    'scheduled',
    'in_progress',
    'completed'
);


ALTER TYPE public.event_status OWNER TO postgres;

--
-- Name: result_status; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.result_status AS ENUM (
    'win_team1',
    'win_team2',
    'draw',
    'in_progress'
);


ALTER TYPE public.result_status OWNER TO postgres;

--
-- Name: transaction_type; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.transaction_type AS ENUM (
    'deposit',
    'withdrawal'
);


ALTER TYPE public.transaction_type OWNER TO postgres;

--
-- Name: create_event_coefficients(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.create_event_coefficients() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
    begin
        insert into coefficient(id, event_id, type) values
            (uuid_generate_v4(), new.id, 'win_team1'),
            (uuid_generate_v4(), new.id, 'win_team2'),
            (uuid_generate_v4(), new.id, 'draw');
    return new;
    end;
    $$;


ALTER FUNCTION public.create_event_coefficients() OWNER TO postgres;

--
-- Name: make_bet(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.make_bet() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
    begin
        insert into transaction
            values (uuid_generate_v4(), new.username, CURRENT_TIMESTAMP, new.amount, 'withdrawal');
    exception
        when others then
            raise exception 'Ошибка при вставке значений в таблицу transaction: %', sqlerrm;
    end;

    return new;
end;
$$;


ALTER FUNCTION public.make_bet() OWNER TO postgres;

--
-- Name: make_closed_bet_transactions(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.make_closed_bet_transactions() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
declare
    win_money numeric;
    win_coefficient uuid;
    coefficient_value numeric;
begin
    if new.status = 'win' then
        select amount, coefficient_id into win_money, win_coefficient
        from bet
        where bet.id = new.id;

        select value into coefficient_value from coefficient
            where win_coefficient = coefficient.id;

        win_money = win_money * coefficient_value;

        raise notice '% % %', win_money, coefficient_value, win_coefficient;
        
        
        insert into transaction
            values (uuid_generate_v4(), new.username, CURRENT_TIMESTAMP, win_money, 'deposit');
        
    end if;
    return new;
end;
$$;


ALTER FUNCTION public.make_closed_bet_transactions() OWNER TO postgres;

--
-- Name: on_update_event_status(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.on_update_event_status() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
    declare
        changed_event_id uuid;
        win_status result_status;
        win_coefficient_id uuid;
        coef_list uuid[];
    begin
        changed_event_id := new.id;
        win_status := new.result;

        select coefficient.id into win_coefficient_id from coefficient
            where cast(win_status as text) = cast(coefficient.type as text) and changed_event_id = coefficient.event_id;


        update bet set status = 'win'
            where bet.coefficient_id = win_coefficient_id;
         RAISE NOTICE 'This is a log message: %', win_coefficient_id;

        select array_agg(coefficient.id) into coef_list from coefficient
                    where changed_event_id = coefficient.event_id;
    raise notice 'list: %', coef_list;
        update bet set status = 'lost'
            where bet.coefficient_id in (
                select coefficient.id from coefficient
                    where changed_event_id = coefficient.event_id
                ) and status != 'win';
        return new;
    end;
    $$;


ALTER FUNCTION public.on_update_event_status() OWNER TO postgres;

--
-- Name: recalculate_coefficients(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.recalculate_coefficients() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
declare
    total_amount_team1 decimal(10, 2) := 0;
    total_amount_team2 decimal(10, 2) := 0;
    total_amount_draw decimal(10, 2) := 0;
    coeff_id uuid;
    v_event_id uuid;
    event_margin decimal(5, 2);
    probability_team1 decimal(5, 2) := 0;
    probability_team2 decimal(5, 2) := 0;
    probability_draw decimal(5, 2) := 0;
    new_value_team1 decimal(5, 2);
    new_value_team2 decimal(5, 2);
    new_value_draw decimal(5, 2);
    total_amount decimal(10, 2);
begin
    coeff_id := new.coefficient_id;
    select coefficient.event_id into v_event_id from coefficient where coefficient.id = coeff_id;

    select event.margin into event_margin
    from event
    where event.id = v_event_id;

    select coalesce(sum(amount), 0) into total_amount_team1
    from bet
    where bet.coefficient_id in (
        select coefficient.id
        from coefficient
        where coefficient.event_id = v_event_id
        and coefficient.type = 'win_team1'
    );

    select coalesce(sum(amount), 0) into total_amount_team2
    from bet
    where bet.coefficient_id in (
        select coefficient.id
        from coefficient
        where coefficient.event_id = v_event_id
        and coefficient.type = 'win_team2'
    );

    select coalesce(sum(amount), 0) into total_amount_draw
    from bet
    where bet.coefficient_id in (
        select coefficient.id
        from coefficient
        where coefficient.event_id = v_event_id
        and coefficient.type = 'draw'
    );

    total_amount := total_amount_team1 + total_amount_team2 + total_amount_draw;

    if total_amount > 0 then
        probability_team1 := total_amount_team1::decimal / total_amount;
        probability_team2 := total_amount_team2::decimal / total_amount;
        probability_draw := total_amount_draw::decimal / total_amount;
    end if;

    if total_amount_team1 = 0 then
        new_value_team1 := 10.00;
    else
        new_value_team1 := 1 / (probability_team1 * (1 + event_margin));
    end if;

    if total_amount_team2 = 0 then
        new_value_team2 := 10.00;
    else
        new_value_team2 := 1 / (probability_team2 * (1 + event_margin));
    end if;

    if total_amount_draw = 0 then
        new_value_draw := 10.00;
    else
        new_value_draw := 1 / (probability_draw * (1 + event_margin));
    end if;

    update coefficient set value = new_value_team1 where coefficient.event_id = v_event_id and coefficient.type = 'win_team1';
    update coefficient set value = new_value_team2 where coefficient.event_id = v_event_id and coefficient.type = 'win_team2';
    update coefficient set value = new_value_draw where coefficient.event_id = v_event_id and coefficient.type = 'draw';

    return new;
end;
$$;


ALTER FUNCTION public.recalculate_coefficients() OWNER TO postgres;

--
-- Name: update_user_balance(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.update_user_balance() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
declare
    balance_tmp decimal;
begin
    if (new.type = 'deposit') then
        update users set balance = balance + new.amount where username = new.username;
    elsif (new.type = 'withdrawal') then
        select balance into balance_tmp from users where username = new.username;
        if (balance_tmp < new.amount) then
            raise exception 'Недостаточно средств на счету для снятия.';
        else
            update users set balance = balance - new.amount where username = new.username;
        end if;
    end if;
    return new;
end;
$$;


ALTER FUNCTION public.update_user_balance() OWNER TO postgres;

--
-- Name: validate_teams_sport_type(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.validate_teams_sport_type() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
declare
    sport1 varchar(50);
    sport2 varchar(50);
begin
    select sport into sport1 from team where id = new.team1_id;
    select sport into sport2 from team where id = new.team2_id;

    if sport1 != sport2 then
        raise exception 'Teams must be of the same sport type';
    end if;

    return new;
end;
$$;


ALTER FUNCTION public.validate_teams_sport_type() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: bet; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bet (
    id uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    coefficient_id uuid NOT NULL,
    username character varying(50) NOT NULL,
    amount numeric(10,2) NOT NULL,
    status public.bet_status DEFAULT 'in_progress'::public.bet_status
);


ALTER TABLE public.bet OWNER TO postgres;

--
-- Name: coefficient; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.coefficient (
    id uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    event_id uuid NOT NULL,
    type public.coefficient_type NOT NULL,
    value numeric(5,2) DEFAULT 1 NOT NULL
);


ALTER TABLE public.coefficient OWNER TO postgres;

--
-- Name: event; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.event (
    id uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    name character varying(100) NOT NULL,
    date_time timestamp without time zone NOT NULL,
    team1_id uuid,
    team2_id uuid,
    sport character varying(50) NOT NULL,
    result public.result_status DEFAULT 'in_progress'::public.result_status,
    margin numeric(5,2)
);


ALTER TABLE public.event OWNER TO postgres;

--
-- Name: team; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.team (
    id uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    name character varying(100),
    country character varying(50),
    sport character varying(50) NOT NULL
);


ALTER TABLE public.team OWNER TO postgres;

--
-- Name: transaction; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.transaction (
    id uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    username character varying(50) NOT NULL,
    date_time timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    amount numeric(10,2) NOT NULL,
    type public.transaction_type NOT NULL
);


ALTER TABLE public.transaction OWNER TO postgres;

--
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    username character varying(50) NOT NULL,
    password character varying(255) NOT NULL,
    full_name character varying(100),
    birth_date date,
    email character varying(100),
    phone character varying(20),
    balance numeric(10,2),
    role character varying
);


ALTER TABLE public.users OWNER TO postgres;

--
-- Data for Name: bet; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.bet (id, coefficient_id, username, amount, status) FROM stdin;
d1e839ef-ab4b-446f-b608-f0c679656928	3cff148f-7935-42c4-b32a-32e34f5791c5	user9	200.00	win
36180558-9fb4-4ee7-a6d2-9d7f678fee4e	3cff148f-7935-42c4-b32a-32e34f5791c5	user10	100.00	win
4b3bfb18-e32f-40b2-ba1a-a5d816bb7093	3cff148f-7935-42c4-b32a-32e34f5791c5	user2	300.00	win
79571664-e520-46f7-8ad4-ebad2c2c9d03	23d7c4d7-0646-4818-b985-c3e68c966ebb	user14	200.00	lost
54a70eac-31e7-47c9-bf8b-3d49ce090eb6	fb91d2e0-ab52-4ee4-af75-5905cb539c8a	user6	100.00	lost
c4ef810b-9529-4b2a-af7b-54b1f6707323	23d7c4d7-0646-4818-b985-c3e68c966ebb	user13	50.00	lost
bbb61848-909e-4d65-bccf-44618fb530ef	fb91d2e0-ab52-4ee4-af75-5905cb539c8a	user5	100.00	lost
0039e68c-ece1-4b3e-bdc6-1a00efab6b67	0938b610-602a-48a7-9b0d-59288b4094c2	user8	100.00	win
1b95bcf1-5323-40cf-a467-925b587b18af	c52ed960-4993-4110-91d1-4988b4aad674	user3	200.00	lost
00f8b62a-6574-4484-87f3-c1c3ba93c0e0	de348fff-51ab-47d3-9449-8c60f2ad96df	user4	500.00	lost
\.


--
-- Data for Name: coefficient; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.coefficient (id, event_id, type, value) FROM stdin;
2af1d556-1ebc-4356-9159-e70b6e5aa68f	03a6e3d7-37d6-4e93-98c1-58391a85c536	win_team1	1.00
ec75bdde-35d9-41e4-9b65-0398e9839e26	03a6e3d7-37d6-4e93-98c1-58391a85c536	win_team2	1.00
03ad1d9f-e318-4263-b264-da4416949a5c	03a6e3d7-37d6-4e93-98c1-58391a85c536	draw	1.00
3cff148f-7935-42c4-b32a-32e34f5791c5	ce70f30a-0e77-4bd6-8df6-1ba54745fee5	win_team1	1.46
23d7c4d7-0646-4818-b985-c3e68c966ebb	ce70f30a-0e77-4bd6-8df6-1ba54745fee5	win_team2	3.47
fb91d2e0-ab52-4ee4-af75-5905cb539c8a	ce70f30a-0e77-4bd6-8df6-1ba54745fee5	draw	4.39
c52ed960-4993-4110-91d1-4988b4aad674	ae90b64a-9c3d-4f4b-ae10-1fe0b4768b77	win_team1	3.48
de348fff-51ab-47d3-9449-8c60f2ad96df	ae90b64a-9c3d-4f4b-ae10-1fe0b4768b77	win_team2	1.38
0938b610-602a-48a7-9b0d-59288b4094c2	ae90b64a-9c3d-4f4b-ae10-1fe0b4768b77	draw	6.69
\.


--
-- Data for Name: event; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.event (id, name, date_time, team1_id, team2_id, sport, result, margin) FROM stdin;
ce70f30a-0e77-4bd6-8df6-1ba54745fee5	Футбольный матч 3	2024-11-28 18:00:00	d704099c-c22b-4d2c-a4b1-7e4924c0f415	a9ca7220-9395-484d-82be-935a24a4707d	футбол	win_team1	0.20
ae90b64a-9c3d-4f4b-ae10-1fe0b4768b77	Баскетбольный матч 1	2024-11-29 12:00:00	1fc948d9-6cfb-45d6-926e-9f60de2e544b	0b9f1ac0-5f7b-47a0-b7b2-f6aac1a14b08	баскетбол	draw	0.15
03a6e3d7-37d6-4e93-98c1-58391a85c536	Футбольный матч	2024-12-02 21:58:04	d704099c-c22b-4d2c-a4b1-7e4924c0f415	a9ca7220-9395-484d-82be-935a24a4707d	футбол	in_progress	0.14
\.


--
-- Data for Name: team; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.team (id, name, country, sport) FROM stdin;
1c5d4204-ba6d-479f-a941-7a0a599ccdf0	Зенит	Россия	футбол
d704099c-c22b-4d2c-a4b1-7e4924c0f415	Спартак	Россия	футбол
fee12af0-d35a-43ce-a0ff-8fcdb28f0283	ЦСКА	Россия	футбол
741e9791-2498-4b2f-9ae5-4ab0042d8829	Локомотив	Россия	футбол
40429857-dd1b-4cbe-946f-84cfc58f45dd	Динамо	Россия	футбол
101b66cc-f631-46f3-9504-618b8c1019cf	Краснодар	Россия	футбол
a9ca7220-9395-484d-82be-935a24a4707d	Ростов	Россия	футбол
4e480af2-b900-4e63-b0d5-f2daff99a466	Рубин	Россия	футбол
bdd078d9-a978-4e12-b8f5-374ce52abdb6	Ахмат	Россия	футбол
57741eca-ebaa-4688-af08-ddea5645fa4b	Сочи	Россия	футбол
1fc948d9-6cfb-45d6-926e-9f60de2e544b	Барселона	Испания	баскетбол
0b9f1ac0-5f7b-47a0-b7b2-f6aac1a14b08	Реал Мадрид	Испания	баскетбол
6160550a-06bf-4e41-a801-4dba807a3fd5	УНИКС	Россия	баскетбол
619db0f0-2df9-4397-a58b-d772f462808c	Химки	Россия	баскетбол
6bdb796d-3273-4111-a1ba-d154350001d4	Зенит БК	Россия	баскетбол
\.


--
-- Data for Name: transaction; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.transaction (id, username, date_time, amount, type) FROM stdin;
f8ae69df-1274-4a27-9f93-1e3ab2368320	user9	2024-11-24 19:38:27.004543	200.00	withdrawal
7e4e244d-fd2a-4fee-a438-9fc2d8bfa8ca	user10	2024-11-24 19:50:16.563843	100.00	withdrawal
30eb8e4d-3ee7-44f0-85bc-c8f4b2479cf9	user5	2024-11-24 19:52:22.498907	2.00	withdrawal
92222158-0839-4585-858a-a04a7cf8a892	user5	2024-11-24 19:53:51.49164	100.00	withdrawal
e6ad3c7b-bad1-4169-96ec-c4fc54998aa0	user6	2024-11-24 19:55:06.449515	500.00	withdrawal
fbaa7f9d-5813-4f2d-bbb3-418d2a8b4c31	user6	2024-11-24 19:59:28.765347	100.00	withdrawal
34d67241-ef8c-4b3a-88a0-87d9fa54477d	user14	2024-11-24 21:56:56.817574	200.00	withdrawal
599b1734-eb64-4c24-8233-35fb50f221ad	user13	2024-11-24 21:58:53.0927	50.00	withdrawal
66996fcb-a036-4910-9dc3-6a39a8d4c27d	user2	2024-11-24 22:37:10.418724	300.00	withdrawal
29dd8fc9-9ac3-45ec-9e72-74baa364ab84	user9	2024-11-24 22:39:58.333331	292.00	deposit
d6980aae-dedc-4b4c-b825-bfdc0e593b2b	user10	2024-11-24 22:39:58.333331	146.00	deposit
ed8a7a98-6a10-4934-bdba-75fa5563dc01	user2	2024-11-24 22:39:58.333331	438.00	deposit
f64a3067-8907-4795-adfc-7074313edbcb	user4	2024-11-25 12:25:34	1000.00	deposit
c60982a0-9790-4599-9a99-cbe008435ac1	user4	2024-11-25 09:26:58.261423	500.00	withdrawal
6d0a8294-577c-4469-93c8-986a5bbc9ae3	user8	2024-11-25 09:27:59.654322	100.00	withdrawal
2b87dc39-8f28-4512-9846-c603ba76da9d	user3	2024-11-25 09:41:22.51198	200.00	withdrawal
6b58344f-440c-4e87-a227-5e8c7b220a05	user8	2024-11-25 09:41:37.704	669.00	deposit
d9657af4-82ef-417f-bf2b-5c213b8c597d	user4	2024-11-25 12:53:22	400.00	deposit
1fde6c60-0a83-446b-a019-80671e74e08b	user10	2024-11-25 09:55:31.776612	900.00	deposit
908d867f-8046-47ef-9fd0-deb822ecd639	user5	2024-12-02 19:05:06.446659	100.00	deposit
712e4a16-563f-40e4-82ff-273d430a69a9	user5	2024-12-02 19:05:58.709627	200.00	deposit
\.


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (username, password, full_name, birth_date, email, phone, balance, role) FROM stdin;
user7	password7	Анастасия Анастасова	1995-11-30	anastasia@example.com	5566778899	800.00	\N
user11	password11	Мария Мариева	1989-08-19	maria@example.com	9900112233	450.00	\N
user12	password12	Кирилл Кириллов	1981-01-29	kirill@example.com	0011223344	500.00	\N
user15	password15	Наталья Натальевна	1987-05-23	natalya@example.com	3344556677	1000.00	\N
user1	password1	Иван Иванов	1990-01-01	ivan@example.com	1234567890	900.00	\N
user6	password6	Андрей Андреев	1980-02-25	andrey@example.com	4455667788	0.00	\N
user14	password14	Василий Васильев	1993-03-17	vasiliy@example.com	2233445566	500.00	\N
user13	password13	Светлана Светланова	1978-10-11	svetlana@example.com	1122334455	250.00	\N
user9	password9	Ольга Ольгина	1982-06-06	olga@example.com	7788990011	992.00	\N
user2	password2	Петр Петров	1985-05-15	petr@example.com	0987654321	438.00	\N
user3	password3	Сергей Сергеев	1975-03-20	sergey@example.com	1122334455	0.00	\N
user8	password8	Екатерина Екатериновна	1991-09-14	ekaterina@example.com	6677889900	1988.00	\N
user4	password4	Алексей Алексеев	1992-07-18	alexey@example.com	2233445566	900.00	\N
user10	password10	Дмитрий Дмитриев	1994-12-22	dmitriy@example.com	8899001122	1196.00	\N
user5	password5	Михаил Михайлов	1988-04-12	mikhail@example.com	3344556677	348.00	\N
\.


--
-- Name: bet bet_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bet
    ADD CONSTRAINT bet_pkey PRIMARY KEY (id);


--
-- Name: coefficient coefficient_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coefficient
    ADD CONSTRAINT coefficient_pkey PRIMARY KEY (id);


--
-- Name: users customer_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT customer_pkey PRIMARY KEY (username);


--
-- Name: event event_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.event
    ADD CONSTRAINT event_pkey PRIMARY KEY (id);


--
-- Name: team team_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.team
    ADD CONSTRAINT team_pkey PRIMARY KEY (id);


--
-- Name: transaction transaction_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transaction
    ADD CONSTRAINT transaction_pkey PRIMARY KEY (id);


--
-- Name: event add_coefficients; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER add_coefficients AFTER INSERT ON public.event FOR EACH ROW EXECUTE FUNCTION public.create_event_coefficients();


--
-- Name: transaction change_user_balance; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER change_user_balance BEFORE INSERT OR UPDATE ON public.transaction FOR EACH ROW EXECUTE FUNCTION public.update_user_balance();


--
-- Name: event check_sport_types; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER check_sport_types BEFORE INSERT OR UPDATE ON public.event FOR EACH ROW EXECUTE FUNCTION public.validate_teams_sport_type();


--
-- Name: bet closed_bet_transaction_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER closed_bet_transaction_trigger AFTER UPDATE OF status ON public.bet FOR EACH ROW EXECUTE FUNCTION public.make_closed_bet_transactions();


--
-- Name: bet on_create_bet; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER on_create_bet BEFORE INSERT ON public.bet FOR EACH ROW EXECUTE FUNCTION public.make_bet();


--
-- Name: bet recalculate_event_coefficients; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER recalculate_event_coefficients AFTER INSERT OR UPDATE ON public.bet FOR EACH ROW EXECUTE FUNCTION public.recalculate_coefficients();


--
-- Name: event update_event_status; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER update_event_status AFTER UPDATE OF result ON public.event FOR EACH ROW EXECUTE FUNCTION public.on_update_event_status();


--
-- Name: bet bet_coefficient_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bet
    ADD CONSTRAINT bet_coefficient_id_fkey FOREIGN KEY (coefficient_id) REFERENCES public.coefficient(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: bet bet_username_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bet
    ADD CONSTRAINT bet_username_fkey FOREIGN KEY (username) REFERENCES public.users(username) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: coefficient coefficient_event_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coefficient
    ADD CONSTRAINT coefficient_event_id_fkey FOREIGN KEY (event_id) REFERENCES public.event(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- Name: event event_team1_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.event
    ADD CONSTRAINT event_team1_id_fkey FOREIGN KEY (team1_id) REFERENCES public.team(id) ON UPDATE CASCADE ON DELETE SET NULL;


--
-- Name: event event_team2_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.event
    ADD CONSTRAINT event_team2_id_fkey FOREIGN KEY (team2_id) REFERENCES public.team(id) ON UPDATE CASCADE ON DELETE SET NULL;


--
-- Name: transaction transaction_username_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transaction
    ADD CONSTRAINT transaction_username_fkey FOREIGN KEY (username) REFERENCES public.users(username) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

