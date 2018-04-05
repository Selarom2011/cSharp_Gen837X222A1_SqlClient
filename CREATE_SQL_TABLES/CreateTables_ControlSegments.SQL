USE [837X222A1]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE Interchange
(
	[Interkey] INT IDENTITY(1,1) NOT NULL,

	[ISA01_AuthorizationInfoQlfr] VARCHAR(2) NULL,
	[ISA02_AuthorizationInfo] VARCHAR(10) NULL,
	[ISA03_SecurityInfoQlfr] VARCHAR(2) NULL,
	[ISA04_SecurityInfo] VARCHAR(10) NULL,
	[ISA05_SenderIdQlfr] VARCHAR(2) NULL,
	[ISA06_SenderId] VARCHAR(15) NULL,
	[ISA07_ReceiverIdQlfr] VARCHAR(2) NULL,
	[ISA08_ReceiverId] VARCHAR(15) NULL,
	[ISA09_Date] VARCHAR(8) NULL,
	[ISA10_Time] VARCHAR(8) NULL,
	[ISA11_RepetitionSeparator] VARCHAR(1) NULL,
	[ISA12_ControlVersionNumber] VARCHAR(5) NULL,
	[ISA13_ControlNumber] VARCHAR(9) NULL,
	[ISA14_AcknowledgmentRequested] VARCHAR(1) NULL,
	[ISA15_UsageIndicator] VARCHAR(1) NULL,
	[ISA16_ComponentElementSeparator] VARCHAR(1) NULL,
	[IEA01_NoOfIncludedFuncGroups] VARCHAR(9) NULL,

	CONSTRAINT PKInterchangeIndex PRIMARY KEY(Interkey)

)

CREATE TABLE FuncGroup
(
	[Groupkey] INT IDENTITY(1,1) NOT NULL,
	[Interkey] INTEGER NULL,

	[GS01_FunctionalIdfrCode] VARCHAR(2) NULL,
	[GS02_SendersCode] VARCHAR(15) NULL,
	[GS03_ReceiversCode] VARCHAR(15) NULL,
	[GS04_Date] VARCHAR(8) NULL,
	[GS05_Time] VARCHAR(8)NULL,
	[GS06_GroupControlNumber] VARCHAR(9) NULL,
	[GS07_ResponsibleAgencyCode] VARCHAR(2) NULL,
	[GS08_VersionReleaseCode] VARCHAR(12) NULL,
	[GE01_NoOfTranSetsIncluded] VARCHAR(9) NULL,

	CONSTRAINT PK_FuncGroupIndex PRIMARY KEY(Groupkey),

	CONSTRAINT FK_FuncGroup_InterReference FOREIGN KEY(Interkey)
		 REFERENCES Interchange(Interkey)
)

GO