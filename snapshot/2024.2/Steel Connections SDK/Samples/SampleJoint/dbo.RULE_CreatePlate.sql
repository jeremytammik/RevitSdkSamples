CREATE TABLE [dbo].[RULE_CreatePlate]
(
	[Key]             INT            NOT NULL,
	[Comment]         NVARCHAR (64)  NULL,
	[Section]         NVARCHAR (128) NULL,
	[PlateThickness]  FLOAT (53)     NULL,
	[PlateLength]     FLOAT (53)     NULL,
	[PlateWidth]      FLOAT (53)     NULL,
	[CutBack]         FLOAT (53)     NULL,
	[AnchorMaterial]  NVARCHAR (50)  NULL,
	[AnchorType]      NVARCHAR (50)  NULL,
	[AnchorAssembly]  NVARCHAR (50)  NULL,
	[AnchorDiameter]  FLOAT (53)     NULL,
	[AnchorLength]    FLOAT (53)     NULL,
	[HoleTolerance]   FLOAT (53)     NULL
)
