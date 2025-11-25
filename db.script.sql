-- create database db_escalonadores;

-- Tabelas

-- prioridade_manchester
CREATE TABLE prioridade_manchester (
  id_prioridade_manchester BIGSERIAL NOT NULL,
  nome_prioridade VARCHAR(255),
  cor VARCHAR(255),
  CONSTRAINT prioridade_manchester_pk PRIMARY KEY (id_prioridade_manchester)
);

-- escalonador
CREATE TABLE escalonador (
  id_escalonador BIGSERIAL NOT NULL,
  nome_escalonador VARCHAR(255),
  CONSTRAINT escalonador_pk PRIMARY KEY (id_escalonador)
);

-- execucao
CREATE TABLE execucao (
  id_execucao BIGSERIAL NOT NULL,
  id_algoritmo BIGINT NOT NULL,
  n_medicos BIGINT NOT NULL,
  n_trocas_contexto BIGINT NOT NULL,
  media_espera DOUBLE PRECISION NOT NULL,
  media_execucao DOUBLE PRECISION NOT NULL,
  media_cpu DOUBLE PRECISION NOT NULL,
  data_execucao TIMESTAMP WITH TIME ZONE NOT NULL,
  CONSTRAINT execucao_pk PRIMARY KEY (id_execucao)
);

-- paciente
CREATE TABLE paciente (
  id_paciente BIGSERIAL NOT NULL,
  id_execucao BIGINT NOT NULL,
  tempo_chegada BIGINT NOT NULL,
  tempo_saida BIGINT NOT NULL,
  tempo_espera BIGINT NOT NULL,
  duracao BIGINT NOT NULL,
  duracao_total BIGINT NOT NULL,
  id_prioridade_manchester BIGINT NOT NULL,
  quantum BIGINT,
  CONSTRAINT paciente_pk PRIMARY KEY (id_paciente)
);

-- escalonador_execucao
CREATE TABLE escalonador_execucao (
  id_escalonador_execucao BIGSERIAL NOT NULL,
  id_execucao BIGINT NOT NULL,
  id_paciente BIGINT,
  contador_medico BIGINT,
  inicio BOOLEAN NOT NULL,
  fim BOOLEAN NOT NULL,
  espera BOOLEAN NOT NULL,
  momento BIGINT NOT NULL,
  CONSTRAINT escalonador_execucao_pk PRIMARY KEY (id_escalonador_execucao)
);

-- Prioridades Manchester (HasData)
INSERT INTO prioridade_manchester (id_prioridade_manchester, nome_prioridade, cor)
VALUES
  (1, 'Emergência', 'Vermelho'),
  (2, 'Muito Urgente', 'Laranja'),
  (3, 'Urgente', 'Amarelo'),
  (4, 'Pouco Urgente', 'Verde'),
  (5, 'Não Urgente', 'Azul');

-- Escalonadores (HasData)
INSERT INTO escalonador (id_escalonador, nome_escalonador)
VALUES
  (1, 'Round Robin'),
  (2, 'Shortest Job First'),
  (3, 'Shortest Remaining Time First'),
  (4, 'Prioridade Não-Preemptivo');