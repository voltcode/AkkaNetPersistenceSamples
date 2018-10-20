CREATE TABLE banker_journal (
  Ordering BIGINT IDENTITY(1,1) NOT NULL,
  PersistenceID NVARCHAR(255) NOT NULL,
  SequenceNr BIGINT NOT NULL,
  Timestamp BIGINT NOT NULL,
  IsDeleted BIT NOT NULL,
  Manifest NVARCHAR(500) NOT NULL,
  Payload VARBINARY(MAX) NOT NULL,
  Tags NVARCHAR(100) NULL,
  SerializerId INTEGER NULL
  CONSTRAINT PK_banker_journal PRIMARY KEY (Ordering),
  CONSTRAINT QU_banker_journal UNIQUE (PersistenceID, SequenceNr)
);

CREATE TABLE banker_snapshot (
  PersistenceID NVARCHAR(255) NOT NULL,
  SequenceNr BIGINT NOT NULL,
  Timestamp DATETIME2 NOT NULL,
  Manifest NVARCHAR(500) NOT NULL,
  Snapshot VARBINARY(MAX) NOT NULL,
  SerializerId INTEGER NULL
  CONSTRAINT PK_banker_snapshot PRIMARY KEY (PersistenceID, SequenceNr)
);

CREATE TABLE banker_metadata (
  PersistenceID NVARCHAR(255) NOT NULL,
  SequenceNr BIGINT NOT NULL,
  CONSTRAINT PK_banker_metadata PRIMARY KEY (PersistenceID, SequenceNr)
);