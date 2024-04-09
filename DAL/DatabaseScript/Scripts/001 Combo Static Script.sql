IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='ComboNames')
CREATE TABLE [dbo].[ComboNames](
	[ComboCode] [nvarchar](20) NOT NULL,
	[ComboName] [nvarchar](50) NOT NULL,
	[CreatedBy] [nvarchar](254) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](254) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[SystemCombo] [char](1) NULL,
 CONSTRAINT [PK_ComboNames] PRIMARY KEY CLUSTERED 
(
	[ComboCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_ComboNames] UNIQUE NONCLUSTERED 
(
	[ComboName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='ComboStaticValues')
CREATE TABLE [dbo].[ComboStaticValues](
	[ComboCode] [nvarchar](20) NOT NULL CONSTRAINT [FK_ComboNames_ComboStaticValues_ComboCode] FOREIGN KEY([ComboCode])
                                         REFERENCES [dbo].[ComboNames] ([ComboCode]),
    [Code] [nvarchar](30) NULL,
	[ComboValue] [nvarchar](100) NOT NULL,
	[SeqNo] [int] NULL,
	[CreatedBy] [nvarchar](254) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](254) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[Active] [char](1) NULL DEFAULT ('N'),
	[Description] [nvarchar](500) NULL,
	[Default] [char](1) NULL DEFAULT ('N'),
	[ComboValueCode]  AS (CONVERT([nvarchar](51),([ComboCode]+'.')+[Code])) PERSISTED NOT NULL,
 CONSTRAINT [PK_ComboStaticValues] PRIMARY KEY CLUSTERED 
(
	[ComboValueCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_ComboStaticValues] UNIQUE NONCLUSTERED 
(
	[ComboCode] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


--IF NOT EXISTS (SELECT 1 FROM ComboNames WHERE ComboCode='PORT_ATTR_TYPE')
--INSERT INTO ComboNames(ComboCode, ComboName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, SystemCombo)
--VALUES
--(
--    N'PORT_ATTR_TYPE', -- ComboCode - nvarchar
--    N'Port Attraction Type', -- ComboName - nvarchar
--    'SYSTEM', -- CreatedBy - nvarchar
--    '2022-01-01', -- CreatedDate - datetime
--    'SYSTEM', -- ModifiedBy - nvarchar
--    '2022-01-01', -- ModifiedDate - datetime
--    'Y' -- SystemCombo - char
--)
--GO