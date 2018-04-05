USE [837X222A1]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TABLE [837X222_Header]
(

	[Headerkey] INT IDENTITY(1,1) NOT NULL,
	[Groupkey] INT NULL,

	[ST01_TranSetIdfrCode] VARCHAR(3) NULL,
	[ST02_TranSetControlNo] VARCHAR(9) NULL,
	[ST03_ImplementConventionRef] VARCHAR(35) NULL,

	[BHT01_HierarchStructCode] VARCHAR(4) NULL,
	[BHT02_TranSetPurposeCode] VARCHAR(2) NULL,
	[BHT03_RefId] VARCHAR(50) NULL,
	[BHT04_Date] VARCHAR(8) NULL,
	[BHT05_Time] VARCHAR(8) NULL,
	[BHT06_TranTypeCode] VARCHAR(2) NULL,

	[NM102_SubmitterTypeQlfr] VARCHAR(1) NULL,
	[NM103_SubmitterLastOrOrganizationName] VARCHAR(60) NULL,
	[NM104_SubmitterFirst] VARCHAR(35) NULL,
	[NM105_SubmitterMiddle] VARCHAR(25) NULL,
	[NM108_SubmitterIdCodeQlfr] VARCHAR(2) NULL,
	[NM109_SubmitterIdCode] VARCHAR(80) NULL,

	[PER02_SubmitterContactName] VARCHAR(60) NULL,
	[PER0X_SubmitterPhoneNo] VARCHAR(15) NULL,
	[PER0X_SubmitterPhoneExtNo] VARCHAR(15) NULL,
	[PER0X_SubmitterFaxNo] VARCHAR(15) NULL,
	[PER0X_SubmitterEmail] VARCHAR(80) NULL,

	[NM102_ReceiverTypeQlfr] VARCHAR(1) NULL,
	[NM103_ReceiverLastOrOrganizationName] VARCHAR(60) NULL,
	[NM108_ReceiverIdCodeQlfr] VARCHAR(2) NULL,
	[NM109_ReceiverIdCode] VARCHAR(80) NULL,


	CONSTRAINT PK_837X222_HeaderIndex PRIMARY KEY(Headerkey),

	CONSTRAINT FK_837X222_HeaderInterReference FOREIGN KEY(Groupkey)
		 REFERENCES FuncGroup(Groupkey)

)

CREATE TABLE [837X222_InfoSource]
(
	[Infosourcekey] INT IDENTITY(1,1) NOT NULL,
	[Headerkey] INT NULL,

	[PRV03_BillingProviderIdCode] VARCHAR(50) NULL,
	[CUR02_CurrencyCode] VARCHAR(3) NULL,

	[NM102_BillingProviderTypeQualifier] VARCHAR(1) NULL,
	[NM103_BillingProviderLastOrOrganizationName] VARCHAR(60) NULL,
	[NM104_BillingProviderFirst] VARCHAR(35) NULL,
	[NM105_BillingProviderMiddle] VARCHAR(25) NULL,
	[NM109_BillingProviderIdCode] VARCHAR(80) NULL,

	[N301_BillingProviderAddr1] VARCHAR(55) NULL,
	[N302_BillingProviderAddr2] VARCHAR(55) NULL,

	[N401_BillingProviderCity] VARCHAR(30) NULL,
	[N402_BillingProviderState] VARCHAR(2) NULL,
	[N403_BillingProviderZip] VARCHAR(15) NULL,
	[N404_BillingProviderCountry] VARCHAR(3) NULL,
	[N407_BillingProviderCountrySubdivision] VARCHAR(3) NULL,

	[REF02_BillingProviderEmployerId] VARCHAR(50) NULL,
	[REF02_BillingProviderSocialSecurityNo] VARCHAR(50) NULL,

	[REF02_BillingProviderStateLicenseNo] VARCHAR(50) NULL,
	[REF02_BillingProviderProviderUPIN] VARCHAR(50) NULL,

	[PER02_BillingProviderContactName] VARCHAR(60) NULL,
	[PER0X_BillingProviderPhoneNo] VARCHAR(15) NULL,
	[PER0X_BillingProviderPhoneExtNo] VARCHAR(15) NULL,
	[PER0X_BillingProviderFaxNo] VARCHAR(15) NULL,
	[PER0X_BillingProviderEmail] VARCHAR(80) NULL,

	[NM102_PayToProviderTypeQlfr] VARCHAR(1) NULL,
	[NM103_PayToProviderLastOrOrganizatioName] VARCHAR(60) NULL,
	
	[N301_PayToProviderAddr1] VARCHAR(55) NULL,
	[N302_PayToProviderAddr2] VARCHAR(55) NULL,

	[N401_PayToProviderCity] VARCHAR(30) NULL,
	[N402_PayToProviderState] VARCHAR(2) NULL,
	[N403_PayToProviderZip] VARCHAR(15) NULL,
	[N404_PayToProviderCountry] VARCHAR(3) NULL,
	[N407_PayToProviderCountrySubdivision] VARCHAR(3) NULL,

	[NM103_PayeeLastOrOrganizationName] VARCHAR(60) NULL,
	[NM108_PayeeIdCodeQlfr] VARCHAR(2) NULL,
	[NM109_PayeeIdCode] VARCHAR(80) NULL,

	[N301_PayeeAddr1] VARCHAR(55) NULL,
	[N302_PayeeAddr2] VARCHAR(55) NULL,

	[N401_PayeeCity] VARCHAR(30) NULL,
	[N402_PayeeState] VARCHAR(2) NULL,
	[N403_PayeeZip] VARCHAR(15) NULL,
	[N404_PayeeCountry] VARCHAR(3) NULL,
	[N407_PayeeCountrySubdivision] VARCHAR(3) NULL,

	[REF02_PayeePayerId] VARCHAR(50) NULL,
	[REF02_PayeeClaimOfficeNo] VARCHAR(50) NULL,
	[REF02_PayeeNAIC_Code] VARCHAR(50) NULL,

	[REF02_PayeeEmployerId] VARCHAR(50) NULL,

	CONSTRAINT PK_InfoSourceIndex PRIMARY KEY(Infosourcekey),

	CONSTRAINT FK_InfoSourecInterReference FOREIGN KEY(Headerkey)
		 REFERENCES [837X222_Header](Headerkey)

)

CREATE TABLE [837X222_Subscriber]
(
	[Subscriberkey] INT IDENTITY(1,1) NOT NULL,
	[InfoSourcekey] int NULL,

	[SBR01_PayerResponsibilitySequenceNumberCode] VARCHAR(1) NOT NULL,
	[SBR02_IndividualRelationshipCode] VARCHAR(2) NOT NULL,
	[SBR03_SubscriberGroup_PolicyNo] VARCHAR(50) NULL,
	[SBR04_SubscriberGroupName] VARCHAR(60) NULL,
	[SBR05_InsuranceTypeCode] VARCHAR(3) NULL,
	[SBR09_ClaimFilingIndicatorCode] VARCHAR(2) NULL,

	[PAT06_PatientDeathDate] VARCHAR(35) NULL,
	[PAT08_PatientWeightPounds] VARCHAR(10) NULL,
	[PAT09_Pregnant] VARCHAR(1) NULL,


	[NM102_SubscriberTypeQualifier] VARCHAR(1) NOT NULL,
	[NM103_SubscriberLastOrOrganizationName] VARCHAR(60) NOT NULL,
	[NM104_SubscriberFirst] VARCHAR(35) NULL,
	[NM105_SubscriberMiddle] VARCHAR(25) NULL,
	[NM107_SubscriberSuffix] VARCHAR(10) NULL,
	[NM108_SubscriberIdCodeQlfr] VARCHAR(2) NULL,
	[NM109_SubscriberIdCode] VARCHAR(80) NULL,

	[N301_SubscriberAddr1] VARCHAR(55) NULL,
	[N302_SubscriberAddr2] VARCHAR(55) NULL,

	[N401_SubscriberCity] VARCHAR(30) NULL,
	[N402_SubscriberState] VARCHAR(2) NULL,
	[N403_SubscriberZip] VARCHAR(15) NULL,
	[N404_SubscriberCountry] VARCHAR(3) NULL,
	[N407_SubscriberCountrySubdivision] VARCHAR(3) NULL,

	[DMG02_SubscriberBirthDate] VARCHAR(35) NULL,
	[DMG03_SubscriberGenderCode] VARCHAR(1) NULL,

	[REF02_SubscriberSocialSecurityNo] VARCHAR(50) NULL,

	[REF02_PropertyCasualtyClaimNo] VARCHAR(50) NULL,

	[PER02_SubscriberContactName] VARCHAR(60) NULL,
	[PER04_SubscriberPhoneNo] VARCHAR(15) NULL,
	[PER06_SubscriberPhoneExtNo] VARCHAR(15) NULL,

	[NM102_PayerTypeQlfr] VARCHAR(1) NOT NULL,
	[NM103_PayerLastOrOrganizatioName] VARCHAR(60) NULL,
	[NM109_PayerIdCode] VARCHAR(80) NULL,
	
	[N301_PayerAddr1] VARCHAR(55) NULL,
	[N302_PayerAddr2] VARCHAR(55) NULL,

	[N401_PayerCity] VARCHAR(30) NULL,
	[N402_PayerState] VARCHAR(2) NULL,
	[N403_PayerZip] VARCHAR(15) NULL,
	[N404_PayerCountry] VARCHAR(3) NULL,
	[N407_PayerCountrySubdivision] VARCHAR(3) NULL,

	[REF02_PayerId] VARCHAR(50) NULL,
	[REF02_EmployerId] VARCHAR(50) NULL,
	[REF02_ClaimOfficeNo] VARCHAR(50) NULL,
	[REF02_NAIC_Code] VARCHAR(50) NULL,

	[REF02_ProviderCommercialNo] VARCHAR(50) NULL,
	[REF02_LocationNo] VARCHAR(50) NULL,

	CONSTRAINT PK_837X222_SubscriberIndex PRIMARY KEY(Subscriberkey),

	CONSTRAINT FK_837X222_SubscriberInterReference FOREIGN KEY(InfoSourcekey)
		 REFERENCES [837X222_InfoSource](InfoSourcekey)

)

CREATE TABLE [837X222_Dependent]
(
	[Dependentkey]  INT IDENTITY(1,1) NOT NULL,
	[Subscriberkey] int NULL,

	[PAT01_IndividualRelationshipCode] VARCHAR(2) NULL,
	[PAT06_PatientDeathDate] VARCHAR(35) NULL,
	[PAT08_PatientWeightPounds] VARCHAR(10) NULL,
	[PAT09_Pregnant] VARCHAR(1) NULL,

	[NM102_PatientTypeQualifier] VARCHAR(1) NULL,
	[NM103_PatientLastOrOrganizationName] VARCHAR(60) NULL,
	[NM104_PatientFirst] VARCHAR(35) NULL,
	[NM105_PatientMiddle] VARCHAR(25) NULL,
	[NM107_PatientSuffix] VARCHAR(10) NULL,

	[N301_PatientAddr1] VARCHAR(55) NULL,
	[N302_PatientAddr2] VARCHAR(55) NULL,

	[N401_PatientCity] VARCHAR(30) NULL,
	[N402_PatientState] VARCHAR(2) NULL,
	[N403_PatientZip] VARCHAR(15) NULL,
	[N404_PatientCountry] VARCHAR(3) NULL,
	[N407_PatientCountrySubdivision] VARCHAR(3) NULL,

	[DMG02_PatientBirthDate] VARCHAR(35) NULL,
	[DMG03_PatientGenderCode] VARCHAR(1) NULL,


	[REF02_PropertyCasualtyClaimNo] VARCHAR(50) NULL,

	[REF02_PatientSocialSecurityNo] VARCHAR(50) NULL,
	[REF02_MemberIdNo] VARCHAR(50) NULL,

	[PER02_PatientContactName] VARCHAR(60) NULL,
	[PER04_PatientPhoneNo] VARCHAR(15) NULL,
	[PER06_PatientPhoneExtNo] VARCHAR(15) NULL,

	CONSTRAINT PK_837X222_DependentIndex PRIMARY KEY(Dependentkey),

	CONSTRAINT FK_837X222_DependentInterReference FOREIGN KEY(Subscriberkey)
		 REFERENCES [837X222_Subscriber](Subscriberkey)

)


CREATE TABLE [837X222_Claims]
(
	[Claimskey] INT IDENTITY(1,1) NOT NULL,
	[Subscriberkey] int NOT NULL,
	[Dependentkey] int NULL,

	[CLM01_PatientControlNo] VARCHAR(38) NULL,
	[CLM02_TotalClaimChargeAmount] VARCHAR(18) NULL,
	[CLM05_01_PlaceOfServiceCode] VARCHAR(2) NULL,
	[CLM05_03_ClaimFrequencyCode] VARCHAR(1) NULL,
	[CLM06_SupplierSignatureIndicator] VARCHAR(1) NULL,
	[CLM07_PlanParticipationCode] VARCHAR(1) NULL,
	[CLM08_BenefitsAssignmentCertIndicator] VARCHAR(1) NULL,
	[CLM09_ReleaseOfInformationCode] VARCHAR(1) NULL,
	[CLM10_PatientSignatureSourceCode] VARCHAR(1) NULL,
	[CLM11_01_RelatedCausesCode] VARCHAR(3) NULL,
	[CLM11_02_RelatedCausesCode] VARCHAR(3) NULL,
	[CLM11_04_AutoAccidentStateCode] VARCHAR(2) NULL,
	[CLM11_05_CountryCode] VARCHAR(3) NULL,
	[CLM112_SpecialProgramCode] VARCHAR(3) NULL,
	[CLM120_DelayReasonCode] VARCHAR(2) NULL,

	[DTP03_OnsetofCurrentIllnessInjuryDate] VARCHAR(8) NULL,
	[DTP03_InitialTreatmentDate] VARCHAR(8) NULL,
	[DTP03_LastSeenDate] VARCHAR(8) NULL,
	[DTP03_AcuteManifestationDate] VARCHAR(8) NULL,
	[DTP03_AccidentDate] VARCHAR(8) NULL,
	[DTP03_LastMenstrualPeriodDate] VARCHAR(8) NULL,
	[DTP03_LastXrayDate] VARCHAR(8) NULL,
	[DTP03_HearVisionPrescriptDate] VARCHAR(8) NULL,
	[DTP03_Disability] VARCHAR(35) NULL,
	[DTP03_InitialDisabilityPeriodStart] VARCHAR(8) NULL,
	[DTP03_InitialDisabilityPeriodEnd] VARCHAR(8) NULL,
	[DTP03_LastWorkedDate] VARCHAR(8) NULL,
	[DTP03_WorkReturnDate] VARCHAR(8) NULL,
	[DTP03_HospitalizationAdmissionDate] VARCHAR(8) NULL,
	[DTP03_HospitalizationDischargeDate] VARCHAR(8) NULL,
	[DTP03_AssumedRelinquishedCareStartDate] VARCHAR(8) NULL,
	[DTP03_AssumedRelinquishedCareEndDate] VARCHAR(8) NULL,
	[DTP03_PropertyCasualtyFirstContactDate] VARCHAR(8) NULL,
	[DTP03_RepricerReceivedDate] VARCHAR(8) NULL,
	
	[PWK01_AttachmentReportTypeCode] VARCHAR(2) NULL,
	[PWK02_AttachmentTransmissionCode] VARCHAR(2) NULL,
	[PWK06_AttachmentControlNo] VARCHAR(80) NULL,

	[CN101_ContractTypeCode] VARCHAR(2) NULL,
	[CN102_ContractAmount] VARCHAR(18) NULL,
	[CN103_ContractPercentage] VARCHAR(6) NULL,
	[CN104_ContractCode] VARCHAR(50) NULL,
	[CN105_TermsDiscountPercent] VARCHAR(6) NULL,
	[CN106_ContractVersionIdentifier] VARCHAR(30) NULL,

	[AMT02_PatientAmountPaid] VARCHAR(18) NULL,

	[REF02_SpecialPaymentReferenceNumber] VARCHAR(50) NULL,
	[REF02_MedicareVersionCode] VARCHAR(50) NULL,
	[REF02_MammographyCertificationNumber] VARCHAR(50) NULL,
	[REF02_ReferralNumber] VARCHAR(50) NULL,
	[REF02_PriorAuthorizationNumber] VARCHAR(50) NULL,
	[REF02_PayerClaimControlNumber] VARCHAR(50) NULL,
	[REF02_ClinicalLabAmendmentNumber] VARCHAR(50) NULL,
	[REF02_RepricedClaimReferenceNumber] VARCHAR(50) NULL,
	[REF02_AdjRepricedClaimReferenceNo] VARCHAR(50) NULL,
	[REF02_InvestigatDeviceExemptIdfr] VARCHAR(50) NULL,
	[REF02_ValueAddedNetworkTraceNumber] VARCHAR(50) NULL,
	[REF02_MedicalRecordNumber] VARCHAR(50) NULL,
	[REF02_DemonstrationProjectIdentifier] VARCHAR(50) NULL,
	[REF02_CarePlanOversightNumber] VARCHAR(50) NULL,

	[K301_FileInformation] VARCHAR(80) NULL,

	[NTE01_NoteReferenceCode] VARCHAR(3) NULL,
	[NTE02_ClaimNoteText] VARCHAR(80) NULL,

	[CR102_PatientWeightPounds] VARCHAR(10) NULL,
	[CR104_Ambulance_Transport_Reason_Code] VARCHAR(1) NULL,
	[CR106_TransportDistanceMiles] VARCHAR(15) NULL,
	[CR109_RoundTripPurposeDescription] VARCHAR(80) NULL,
	[CR110_StretcherPurposeDescription] VARCHAR(80) NULL,

	[CR208_NatureOfConditionCode] VARCHAR(1) NULL,
	[CR210_PatientConditionDescription] VARCHAR(80) NULL,
	[CR211_PatientConditionDescription2] VARCHAR(80) NULL,


	[CRC02_AmbulanceConditionIndicator] VARCHAR(1) NULL,
	[CRC03_AmbulanceConditionCode] VARCHAR(3) NULL,
	[CRC04_AmbulanceConditionCode2] VARCHAR(3) NULL,
	[CRC05_AmbulanceConditionCode3] VARCHAR(3) NULL,
	[CRC06_AmbulanceConditionCode4] VARCHAR(3) NULL,
	[CRC07_AmbulanceConditionCode5] VARCHAR(3) NULL,

	[CRC01_PatientVisionCodeCategory] VARCHAR(2) NULL,
	[CRC02_PatientVisionConditionIndicator] VARCHAR(1) NULL,
	[CRC03_PatientVisionConditionCode] VARCHAR(3) NULL,
	[CRC04_PatientVisionConditionCode2] VARCHAR(3) NULL,
	[CRC05_PatientVisionConditionCode3] VARCHAR(3) NULL,
	[CRC06_PatientVisionConditionCode4] VARCHAR(3) NULL,
	[CRC07_PatientVisionConditionCode5] VARCHAR(3) NULL,

	[CRC02_HomeboundConditionCode] VARCHAR(1) NULL,
	[CRC03_HomeboundIndicator] VARCHAR(3) NULL,

	[CRC02_EPSDT_ConditionCodeAppliesIndicator] VARCHAR(1) NULL,
	[CRC03_EPSDT_ConditionIndicator] VARCHAR(3) NULL,
	[CRC04_EPSDT_ConditionIndicator2] VARCHAR(3) NULL,
	[CRC05_EPSDT_ConditionIndicator3] VARCHAR(3) NULL,

	[HI01_01_DiagnosisTypeCode] VARCHAR(3) NULL,
	[HI01_02_DiagnosisCode] VARCHAR(30) NULL,
	[HI02_01_DiagnosisTypeCode2] VARCHAR(3) NULL,
	[HI02_02_DiagnosisCode2] VARCHAR(30) NULL,
	[HI03_01_DiagnosisTypeCode3] VARCHAR(3) NULL,
	[HI03_02_DiagnosisCode3] VARCHAR(30) NULL,
	[HI04_01_DiagnosisTypeCode4] VARCHAR(3) NULL,
	[HI04_02_DiagnosisCode4] VARCHAR(30) NULL,
	[HI05_01_DiagnosisTypeCode5] VARCHAR(3) NULL,
	[HI05_02_DiagnosisCode5] VARCHAR(30) NULL,
	[HI06_01_DiagnosisTypeCode6] VARCHAR(3) NULL,
	[HI06_02_DiagnosisCode6] VARCHAR(30) NULL,
	[HI07_01_DiagnosisTypeCode7] VARCHAR(3) NULL,
	[HI07_02_DiagnosisCode7] VARCHAR(30) NULL,
	[HI08_01_DiagnosisTypeCode8] VARCHAR(3) NULL,
	[HI08_02_DiagnosisCode8] VARCHAR(30) NULL,
	[HI09_01_DiagnosisTypeCode9] VARCHAR(3) NULL,
	[HI09_02_DiagnosisCode9] VARCHAR(30) NULL,
	[HI10_01_DiagnosisTypeCode10] VARCHAR(3) NULL,
	[HI10_02_DiagnosisCode10] VARCHAR(30) NULL,
	[HI11_01_DiagnosisTypeCode11] VARCHAR(3) NULL,
	[HI11_02_DiagnosisCode11] VARCHAR(30) NULL,
	[HI12_01_DiagnosisTypeCode12] VARCHAR(3) NULL,
	[HI12_02_DiagnosisCode12] VARCHAR(30) NULL,

	[HI01_02_AnesthesiaSurgicalPrincipleProcedure] VARCHAR(30) NULL,
	[HI02_02_AnesthesiaSurgicalProcedure] VARCHAR(30) NULL,
	
	[HI01_02_ConditionCode1] VARCHAR(30) NULL,
	[HI02_02_ConditionCode2] VARCHAR(30) NULL,
	[HI03_02_ConditionCode3] VARCHAR(30) NULL,
	[HI04_02_ConditionCode4] VARCHAR(30) NULL,
	[HI05_02_ConditionCode5] VARCHAR(30) NULL,
	[HI06_02_ConditionCode6] VARCHAR(30) NULL,
	[HI07_02_ConditionCode7] VARCHAR(30) NULL,
	[HI08_02_ConditionCode8] VARCHAR(30) NULL,
	[HI09_02_ConditionCode9] VARCHAR(30) NULL,
	[HI10_02_ConditionCode10] VARCHAR(30) NULL,
	[HI11_02_ConditionCode11] VARCHAR(30) NULL,
	[HI12_02_ConditionCode12] VARCHAR(30) NULL,

	[HCP01_PricingMethodology] VARCHAR(2) NULL,
	[HCP02_RepricedAllowedAmount] VARCHAR(18) NULL,
	[HCP03_RepricedSavingAmount] VARCHAR(18) NULL,
	[HCP04_RepricingOrganizationIdentifier] VARCHAR(50) NULL,
	[HCP05_RepricingPerDiemFlatRateAmount] VARCHAR(9) NULL,
	[HCP06_RepricedApprovAmbPatientGroupCode] VARCHAR(50) NULL,
	[HCP07_RepricedApprovAmbPatientGroupAmount] VARCHAR(18) NULL,
	[HCP13_RejectReasonCode] VARCHAR(2) NULL,
	[HCP14_PolicyComplianceCode] VARCHAR(2) NULL,
	[HCP15_ExceptionCode] VARCHAR(2) NULL,

	[NM103_ReferringProviderLastName] VARCHAR(60) NULL,
	[NM104_ReferringProviderLastFirst] VARCHAR(35) NULL,
	[NM105_ReferringProviderLastMiddle] VARCHAR(25) NULL,
	[NM107_ReferringProviderLastSuffix] VARCHAR(10) NULL,
	[NM109_ReferringProviderIdentifier] VARCHAR(80) NULL,

	[REF01_ReferringProviderSecondaryIdQlfr] VARCHAR(3) NULL,
	[REF02_ReferringProviderSecondaryId] VARCHAR(50) NULL,

	[NM103_PrimaryCareProviderLastName] VARCHAR(60) NULL,
	[NM104_PrimaryCareProviderLastFirst] VARCHAR(35) NULL,
	[NM105_PrimaryCareProviderLastMiddle] VARCHAR(25) NULL,
	[NM107_PrimaryCareProviderLastSuffix] VARCHAR(10) NULL,
	[NM109_PrimaryCareProviderIdentifier] VARCHAR(80) NULL,

	[NM102_RenderingProviderTypeQualifier] VARCHAR(1) NULL,
	[NM103_RenderingProviderLastOrOrganizationName] VARCHAR(60) NULL,
	[NM104_RenderingProviderFirst] VARCHAR(35) NULL,
	[NM105_RenderingProviderMiddle] VARCHAR(25) NULL,
	[NM107_RenderingProviderSuffix] VARCHAR(10) NULL,
	[NM109_RenderingProviderIdentifier] VARCHAR(80) NULL,

	[PRV03_ProviderTaxonomyCode] VARCHAR(50) NULL,

	[REF01_RenderingProviderSecondaryIdQlfr] VARCHAR(3) NULL,
	[REF02_RenderingProviderSecondaryId] VARCHAR(50) NULL,

	[NM103_LabFacilityName] VARCHAR(60) NULL,
	[NM109_LabFacilityIdentifier] VARCHAR(80) NULL,

	[N301_LabFacilityAddress1] VARCHAR(55) NULL,
	[N302_LabFacilityAddress2] VARCHAR(55) NULL,

	[N401_LabFacilityCity] VARCHAR(30) NULL,
	[N402_LabFacilityState] VARCHAR(2) NULL,
	[N403_LabFacilityZip] VARCHAR(15) NULL,
	[N404_LabFacilityCountryCode] VARCHAR(3) NULL,
	[N407_LabFacilityCountrySubdivisionCode] VARCHAR(3) NULL,

	[REF01_LabFacilityyIdQualifier] VARCHAR(3) NULL,
	[REF02_LabFacilityIdentification] VARCHAR(50) NULL,

	[PER02_LabFacilityContactName] VARCHAR(60) NULL,
	[PER04_LabFacilityTelephoneNumber] VARCHAR(15) NULL,
	[PER06_LabFacilityExtensionNumber] VARCHAR(15) NULL,

	[NM103_SupervisingPhysicianLastName] VARCHAR(60) NULL,
	[NM104_SupervisingPhysicianFirst] VARCHAR(35) NULL,
	[NM105_SupervisingPhysicianMiddle] VARCHAR(25) NULL,
	[NM107_SupervisingPhysicianSuffix] VARCHAR(10) NULL,
	[NM109_SupervisingPhysicianIdentifier] VARCHAR(80) NULL,

	[REF01_SupervisingPhysicianSecondaryIdQlfr] VARCHAR(3) NULL,
	[REF02_SupervisingPhysicianIdSecondaryId] VARCHAR(50) NULL,

	[N301_AmbulancePickupAddress1] VARCHAR(55) NULL,
	[N302_AmbulancePickupAddress2] VARCHAR(55) NULL,

	[N401_AmbulancePickupCity] VARCHAR(30) NULL,
	[N402_AmbulancePickupState] VARCHAR(2) NULL,
	[N403_AmbulancePickupZip] VARCHAR(15) NULL,
	[N404_AmbulancePickupCountryCode] VARCHAR(3) NULL,
	[N407_AmbulancePickupCountrySubdivisionCode] VARCHAR(3) NULL,

	[N301_AmbulanceDropOffAddress1] VARCHAR(55) NULL,
	[N302_AmbulanceDropOffAddress2] VARCHAR(55) NULL,
	
	[N401_AmbulanceDropOffCity] VARCHAR(30) NULL,
	[N402_AmbulanceDropOffState] VARCHAR(2) NULL,
	[N403_AmbulanceDropOffZip] VARCHAR(15) NULL,
	[N404_AmbulanceDropOffCountryCode] VARCHAR(3) NULL,
	[N407_AmbulanceDropOffCountrySubdivisionCode] VARCHAR(3) NULL,

	CONSTRAINT PK_837X222_ClaimsIndex PRIMARY KEY(Claimskey),

	CONSTRAINT FK_837X222_ClaimsSubscriberInterReference FOREIGN KEY(Subscriberkey)
		 REFERENCES [837X222_Subscriber](Subscriberkey),

	
)

CREATE TABLE [837X222_OtherSubscriberInfo]
(
	[OtherSubscriberInfokey] INT IDENTITY(1,1) NOT NULL,
	[Claimskey] int NULL,

	[SBR01_PayerResponsibSeqNoCode] VARCHAR(1) NULL,
	[SBR02_IndividualRelationshipCode] VARCHAR(2) NULL,
	[SBR03_ReferenceIdentification] VARCHAR(50) NULL,
	[SBR04_OtherInsuredGroupName] VARCHAR(60) NULL,
	[SBR05_InsuranceTypeCode] VARCHAR(3) NULL,
	[SBR09_ClaimFilingIndicatorCode] VARCHAR(2) NULL,

	[CAS01_ClaimAdjustmentGroupCode] VARCHAR(2) NULL,
	[CAS02_AdjustmentReasonCode] VARCHAR(5) NULL,
	[CAS03_AdjustmentAmount] VARCHAR(18) NULL,
	[CAS04_AdjustmentQuantity] VARCHAR(15) NULL,
	[CAS05_AdjustmentReasonCode2] VARCHAR(5) NULL,
	[CAS06_AdjustmentAmount2] VARCHAR(18) NULL,
	[CAS07_AdjustmentQuantity2] VARCHAR(15) NULL,
	[CAS08_AdjustmentReasonCode3] VARCHAR(5) NULL,
	[CAS09_AdjustmentAmount3] VARCHAR(18) NULL,
	[CAS10_AdjustmentQuantity3] VARCHAR(15) NULL,
	[CAS11_AdjustmentReasonCode4] VARCHAR(5) NULL,
	[CAS12_AdjustmentAmount4] VARCHAR(18) NULL,
	[CAS13_AdjustmentQuantity4] VARCHAR(15) NULL,
	[CAS14_AdjustmentReasonCode5] VARCHAR(5) NULL,
	[CAS15_AdjustmentAmount5] VARCHAR(18) NULL,
	[CAS16_AdjustmentQuantity5] VARCHAR(15) NULL,
	[CAS17_AdjustmentReasonCode6] VARCHAR(5) NULL,
	[CAS18_AdjustmentAmount6] VARCHAR(18) NULL,
	[CAS19_AdjustmentQuantity6] VARCHAR(15) NULL,

	[AMT02_PayorAmountPaid] VARCHAR(18) NULL,

	[AMT02_NonCoveredChargedAmount] VARCHAR(18) NULL,

	[AMT02_RemainingPatientLiability] VARCHAR(18) NULL,

	[OI03_BenefitsAssignmentCertIndicator] VARCHAR(1) NULL,
	[OI04_PatientSignatureSourceCode] VARCHAR(1) NULL,
	[OI06_ReleaseOfInformationCode] VARCHAR(1) NULL,

	[MOA01_ReimbursementRate] VARCHAR(10) NULL,
	[MOA02_HCPCS_PayableAmount] VARCHAR(18) NULL,
	[MOA03_ClaimPaymentRemarkCode] VARCHAR(50) NULL,
	[MOA04_ClaimPaymentRemarkCode2] VARCHAR(50) NULL,
	[MOA05_ClaimPaymentRemarkCode3] VARCHAR(50) NULL,
	[MOA06_ClaimPaymentRemarkCode4] VARCHAR(50) NULL,
	[MOA07_ClaimPaymentRemarkCode5] VARCHAR(50) NULL,
	[MOA08_EndStageRenalDiseasePaymntAmnt] VARCHAR(18) NULL,
	[MOA09_NonPayableProfessionComponentBill] VARCHAR(18) NULL,

	[NM102_OtherInsuredEntityTypeQlfr] VARCHAR(1) NULL,
	[NM103_OtherInsuredLastName] VARCHAR(60) NULL,
	[NM104_OtherInsuredFirst] VARCHAR(35) NULL,
	[NM105_OtherInsuredMiddle] VARCHAR(25) NULL,
	[NM107_OtherInsuredSuffix] VARCHAR(10) NULL,
	[NM108_OtherInsuredIdQlfr] VARCHAR(2) NULL,
	[NM109_OtherInsuredID] VARCHAR(80) NULL,

	[N301_OtherInsuredAddress] VARCHAR(55) NULL,
	[N302_OtherInsuredAddress2] VARCHAR(55) NULL,

	[N401_OtherInsuredCity] VARCHAR(30) NULL,
	[N402_OtherInsuredState] VARCHAR(2) NULL,
	[N403_OtherInsuredZip] VARCHAR(15) NULL,
	[N404_OtherInsuredCountryCode] VARCHAR(3) NULL,
	[N407_OtherInsuredCountrySubdivision] VARCHAR(3) NULL,

	[REF02_OtherInsuredSecondaryID] VARCHAR(50) NULL,

	[NM103_OtherPayerOrganizationName] VARCHAR(60) NULL,
	[NM108_OtherPayerCodeQlfr] VARCHAR(2) NULL,
	[NM109_OtherPayerPrimaryID] VARCHAR(80) NULL,

	[N301_OtherPayerAddress1] VARCHAR(55) NULL,
	[N302_OtherPayerAddress2] VARCHAR(55) NULL,

	[N401_OtherPayerCity] VARCHAR(30) NULL,
	[N402_OtherPayerState] VARCHAR(2) NULL,
	[N403_OtherPayerZip] VARCHAR(15) NULL,
	[N404_OtherPayerCountryCode] VARCHAR(3) NULL,
	[N407_OtherPayerCountrySubdivision] VARCHAR(3) NULL,

	[DTP03_OtherPayerPaymentDate] VARCHAR(35) NULL,

	[REF01_OtherPayerSecondaryIdQlfr] VARCHAR(3) NULL,
	[REF02_OtherPayerSecondaryID] VARCHAR(50) NULL,

	[REF02_OtherPayerPriorAuthorizationNo] VARCHAR(50) NULL,

	[REF02_OtherPayerReferralNo] VARCHAR(50) NULL,

	[REF02_OtherPayerClaimAdjustmentIndicator] VARCHAR(50) NULL,

	[REF02_OtherPayerClaimControlNo] VARCHAR(50) NULL,

	[REF01_OtherReferringProviderIdQlfr] VARCHAR(3) NULL,
	[REF02_OtherReferringProviderID] VARCHAR(50) NULL,

	[REF01_OtherPrimaryCareProviderIdQlfr] VARCHAR(3) NULL,
	[REF02_OtherPrimaryCareProviderID] VARCHAR(50) NULL,

	[REF01_OtherRenderingProviderIdQlfr] VARCHAR(3) NULL,
	[REF02_OtherRenderingProviderID] VARCHAR(50) NULL,

	[REF01_OtherServiceLocationIdQlfr] VARCHAR(3) NULL,
	[REF02_OtherServiceLocationID] VARCHAR(50) NULL,

	[REF01_OtherSupervisingPhysicianIdQlfr] VARCHAR(3) NULL,
	[REF02_OtherSupervisingPhysicianID] VARCHAR(50) NULL,

	[NM102_OtherBillingProvideEntityTypeQlfr] VARCHAR(1) NULL,
	[REF01_OtherBillingProviderIdQlfr] VARCHAR(3) NULL,
	[REF02_OtherBillingProviderID] VARCHAR(50) NULL,

	CONSTRAINT PK_837X222_OtherSubscriberInfoIndex PRIMARY KEY(OtherSubscriberInfokey),

	CONSTRAINT FK_837X222_OtherSubscriberInfoInterReference FOREIGN KEY(Claimskey)
		 REFERENCES [837X222_Claims](Claimskey)
)

CREATE TABLE [837X222_ServiceLine]
(
	[ServiceLinekey] INT IDENTITY(1,1) NOT NULL,
	[Claimskey] int NOT NULL,

	[LX01_AssignedNumber] VARCHAR(6) NULL,

	[SV101_01_ProductServiceIdQualifier] VARCHAR(2) NULL,
	[SV101_02_ProcedureCode] VARCHAR(48) NULL,
	[SV101_03_ProcedureModifier1] VARCHAR(2) NULL,
	[SV101_04_ProcedureModifier2] VARCHAR(2) NULL,
	[SV101_05_ProcedureModifier3] VARCHAR(2) NULL,
	[SV101_06_ProcedureModifier4] VARCHAR(2) NULL,
	[SV101_07_ServiceDescription] VARCHAR(80) NULL,
	[SV102_LineItemChargeAmount] VARCHAR(18) NULL,
	[SV103_UnitForMeasurement_Code] VARCHAR(2) NULL,
	[SV104_ServiceUnitCount] VARCHAR(15) NULL,
	[SV105_PlaceOfServiceCode] VARCHAR(2) NULL,
	[SV107_01_DiagnosisCodePointer1] VARCHAR(2) NULL,
	[SV107_02_DiagnosisCodePointer2] VARCHAR(2) NULL,
	[SV107_03_DiagnosisCodePointer3] VARCHAR(2) NULL,
	[SV107_04_DiagnosisCodePointer4] VARCHAR(2) NULL,
	[SV109_EmergencyIndicator] VARCHAR(1) NULL,
	[SV111_EPSDT_Indicator] VARCHAR(1) NULL,
	[SV112_FamilyPlanningIndicator] VARCHAR(1) NULL,
	[SV115_CopayStatusCode] VARCHAR(1) NULL,

	[SV501_01_ProcedureIdentifier] VARCHAR(2) NULL,
	[SV501_02_ProcedureCode] VARCHAR(48) NULL,
	[SV503_DaysLengthOfMedicalNecissity] VARCHAR(15) NULL,
	[SV504_DME_RentalPrice] VARCHAR(18) NULL,
	[SV505_DME_PurchasePrice] VARCHAR(18) NULL,
	[SV506_RentalUnitPriceInidcator] VARCHAR(1) NULL,

	[PWK01_LineSupplementAttachReportTypeCode] VARCHAR(2) NULL,
	[PWK02_LineSupplementAttachTransmissnCode] VARCHAR(2) NULL,
	[PWK06_LineSupplementAttachControlNo] VARCHAR(80) NULL,

	[PWK02_DMERC_CMN_AttachTransmissnCode] VARCHAR(2) NULL,

	[CR102_PatientWeightLbs] VARCHAR(10) NULL,
	[CR104_AmbulanceTransportReasonCode] VARCHAR(1) NULL,
	[CR106_TransportDistanceMiles] VARCHAR(15) NULL,
	[CR109_RoundTripPurposeDescription] VARCHAR(80) NULL,
	[CR110_StretcherPurposeDescription] VARCHAR(80) NULL,

	[CR301_DMERC_CertificationTypeCode] VARCHAR(1) NULL,
	[CR303_DME_DurationMonths] VARCHAR(15) NULL,

	[CRC02_AmbulanceCertConditionIndicator] VARCHAR(1) NULL,
	[CRC03_AmbulanceCertConditionCode1] VARCHAR(3) NULL,
	[CRC04_AmbulanceCertConditionCode2] VARCHAR(3) NULL,
	[CRC05_AmbulanceCertConditionCode3] VARCHAR(3) NULL,
	[CRC06_AmbulanceCertConditionCode4] VARCHAR(3) NULL,
	[CRC07_AmbulanceCertConditionCode5] VARCHAR(3) NULL,

	[CRC02_HospiceEmployedProviderIndicator] VARCHAR(1) NULL,

	[CRC02_DMERC_ConditionIndicator] VARCHAR(1) NULL,
	[CRC03_DMERC_ConditionCode1] VARCHAR(3) NULL,
	[CRC04_DMERC_ConditionCode2] VARCHAR(3) NULL,

	[DTP03_ServiceDate] VARCHAR(35) NULL,

	[DTP03_PrescriptionDate] VARCHAR(35) NULL,

	[DTP03_CertificationRevisionDate] VARCHAR(35) NULL,

	[DTP03_BeginTherapyDate] VARCHAR(35) NULL,

	[DTP03_LastCertificationDate] VARCHAR(35) NULL,

	[DTP03_LastSeenDate] VARCHAR(35) NULL,

	[DTP01_LastTestQualifier] VARCHAR(3) NULL,
	[DTP03_LastTestDate] VARCHAR(35) NULL,

	[DTP03_ShippedDate] VARCHAR(35) NULL,

	[DTP03_LastXrayDate] VARCHAR(35) NULL,

	[DTP03_InitialTreatmentDate] VARCHAR(35) NULL,

	[QTY02_AmbulancePatientCount] VARCHAR(15) NULL,

	[QTY02_ObstetricAdditionalUnits] VARCHAR(15) NULL,

	[MEA01_TestResultReferenceIdCode] VARCHAR(2) NULL,
	[MEA02_TestResultQualifier] VARCHAR(3) NULL,
	[MEA03_TestResultValue] VARCHAR(20) NULL,

	[CN101_ContractTypeCode] VARCHAR(2) NULL,
	[CN102_ContractAmount] VARCHAR(18) NULL,
	[CN103_ContractPercentage] VARCHAR(6) NULL,
	[CN104_ContractCode] VARCHAR(50) NULL,
	[CN105_ContractTermsDiscPercent] VARCHAR(6) NULL,
	[CN106_ContractVersionIdentifier] VARCHAR(30) NULL,

	[REF02_RepricedLineItemRefNo] VARCHAR(50) NULL,

	[REF02_AdjustedRepricedLineItemRefNo] VARCHAR(50) NULL,

	[REF02_PriorAuthorizationNumber] VARCHAR(50) NULL,
	[REF04_02_PriorAuthorizOtherPayrPrimaryID] VARCHAR(50) NULL,

	[REF02_LineItemControlNumber] VARCHAR(50) NULL,

	[REF02_MammographyCertificationNumber] VARCHAR(50) NULL,

	[REF02_ClinicalLabImproveAmendment] VARCHAR(50) NULL,

	[REF02_ReferringCLIA_Number] VARCHAR(50) NULL,

	[REF02_ImmunizationBatchNumber] VARCHAR(50) NULL,

	[REF02_ReferralNumber] VARCHAR(50) NULL,
	[REF04_02_ReferralOtherPayerPrimaryID] VARCHAR(50) NULL,

	[AMT02_SalesTaxAmount] VARCHAR(18) NULL,

	[AMT02_PostageClaimedAmount] VARCHAR(18) NULL,

	[K301_FileInformation] VARCHAR(80) NULL,

	[NTE01_LineNoteReferenceCode] VARCHAR(3) NULL,
	[NTE02_LineNoteText] VARCHAR(80) NULL,

	[NTE01_ThirdPartyNoteCode] VARCHAR(3) NULL,
	[NTE02_ThirdPartyText] VARCHAR(80) NULL,

	[PS101_PurchasedServiceProviderIdfr] VARCHAR(50) NULL,
	[PS102_PurchasedServiceChargeAmnt] VARCHAR(18) NULL,

	[HCP01_LineRepriceCode] VARCHAR(2) NULL,
	[HCP02_RepricedAllowedAmount] VARCHAR(18) NULL,
	[HCP03_RepricedSavingAmount] VARCHAR(18) NULL,
	[HCP04_RepricingOrganizationID] VARCHAR(50) NULL,
	[HCP05_RepricingPerDiemFlatRateAmount] VARCHAR(9) NULL,
	[HCP06_RepricedApprovedAmbPatientGrpCode] VARCHAR(50) NULL,
	[HCP07_RepricedApprovedAmbPatientGroupAmnt] VARCHAR(18) NULL,
	[HCP09_RepricedServiceIdQualifier] VARCHAR(2) NULL,
	[HCP10_RepricedApprovedHCPCS_Code] VARCHAR(48) NULL,
	[HCP11_RepricedUnitMeasurementCode] VARCHAR(2) NULL,
	[HCP12_RepricedApprovedServiceUnitCount] VARCHAR(15) NULL,
	[HCP13_RepricedRejectReasonCode] VARCHAR(2) NULL,
	[HCP14_RepricedPolicyComplianceCode] VARCHAR(2) NULL,
	[HCP15_RepricedExceptionCode] VARCHAR(2) NULL,

	[LIN03_NationalDrugCode] VARCHAR(48) NULL,

	[CTP04_NationalDrugUnitCount] VARCHAR(15) NULL,
	[CTP05_01_UnitMeasurementCode] VARCHAR(2) NULL,

	[REF01_PrescriptionQualifier] VARCHAR(3) NULL,
	[REF02_PrescriptionNumber] VARCHAR(50) NULL,

	[NM102_RenderingProviderEntityTypeQlfr] VARCHAR(1) NULL,
	[NM103_RenderingProviderNameLastOrg] VARCHAR(60) NULL,
	[NM104_RenderingProviderFirst] VARCHAR(35) NULL,
	[NM105_RenderingProviderMiddle] VARCHAR(25) NULL,
	[NM107_RenderingProviderSuffix] VARCHAR(10) NULL,
	[NM109_RenderingProviderID] VARCHAR(80) NULL,

	[PRV03_RenderingProviderTaxonomyCode] VARCHAR(50) NULL,

	[REF01_RenderingProviderSecondaryQlfr] VARCHAR(3) NULL,
	[REF02_RenderingProviderSecondaryID] VARCHAR(50) NULL,
	[REF04_01_ReferenceIdQlfr] VARCHAR(3) NULL,
	[REF04_02_RenderingProviderSecondaryPayerID] VARCHAR(50) NULL,

	[NM102_PurchasedServiceProviderEntityType] VARCHAR(1) NULL,
	[NM109_PurchasedServiceProviderID] VARCHAR(80) NULL,

	[REF01_PurchasedServicProvidrSecondryIdQlfr] VARCHAR(3) NULL,
	[REF02_PurchasedServiceProviderSecondaryID] VARCHAR(50) NULL,
	[REF04_02_PurchsdServcProvdrSecndryPayrIdNo] VARCHAR(50) NULL,

	[NM103_ServiceFacilityName] VARCHAR(60) NULL,
	[NM109_ServiceFacilityID] VARCHAR(80) NULL,

	[N301_ServiceFacilityAddress1] VARCHAR(55) NULL,
	[N302_ServiceFacilityAddress2] VARCHAR(55) NULL,

	[N401_ServiceFacilityCity] VARCHAR(30) NULL,
	[N402_ServiceFacilityState] VARCHAR(2) NULL,
	[N403_ServiceFacilityZip] VARCHAR(15) NULL,
	[N404_ServiceFacilityCountryCode] VARCHAR(3) NULL,
	[N407_ServiceFacilityCountrySubdivision] VARCHAR(3) NULL,

	[REF01_ServiceFacilitySecondaryIdQlfr] VARCHAR(3) NULL,
	[REF02_ServiceFacilitySecondaryID] VARCHAR(50) NULL,
	[REF04_02_ServiceFaciltySecondryPayrIdNo] VARCHAR(50) NULL,
	
	[NM103_SupervisingProviderLastName] VARCHAR(60) NULL,
	[NM104_SupervisingProviderFirst] VARCHAR(35) NULL,
	[NM105_SupervisingProviderMiddle] VARCHAR(25) NULL,
	[NM107_SupervisingProviderSuffix] VARCHAR(10) NULL,
	[NM109_SupervisingProviderID] VARCHAR(80) NULL,

	[REF01_SupervisingProvidrSecondryIdQlfr] VARCHAR(3) NULL,
	[REF02_SupervisingProviderSecondaryID] VARCHAR(50) NULL,
	[REF04_02_SupervisngProvdrSecndryPayrIdNo] VARCHAR(50) NULL,

	
	[NM103_OrderingProviderLastName] VARCHAR(60) NULL,
	[NM104_OrderingProviderFirst] VARCHAR(35) NULL,
	[NM105_OrderingProviderMiddle] VARCHAR(25) NULL,
	[NM107_OrderingProviderSuffix] VARCHAR(10) NULL,
	[NM109_OrderingProviderID] VARCHAR(80) NULL,

	[N301_OrderingProviderAddress1] VARCHAR(55) NULL,
	[N302_OrderingProviderAddress2] VARCHAR(55) NULL,

	[N401_OrderingProviderCity] VARCHAR(30) NULL,
	[N402_OrderingProviderState] VARCHAR(2) NULL,
	[N403_OrderingProviderZip] VARCHAR(15) NULL,
	[N404_OrderingProviderCountryCode] VARCHAR(3) NULL,
	[N407_OrderingProviderCountrySubdivision] VARCHAR(3) NULL,

	[REF01_OrderingProviderSecondaryIdQlfr] VARCHAR(3) NULL,
	[REF02_OrderingProviderSecondaryID] VARCHAR(50) NULL,
	[REF04_02_OrderingProviderSecondaryIdNo] VARCHAR(50) NULL,

	[PER02_OrderingProviderContactName] VARCHAR(60) NULL,
	[PER0X_OrderingProviderContactTelephone] VARCHAR(15) NULL,
	[PER0X_OrderingProviderContactExtension] VARCHAR(15) NULL,
	[PER0X_OrderingProviderContactFax] VARCHAR(15) NULL,
	[PER0X_OrderingProviderContactEmail] VARCHAR(80) NULL,

	[NM101_ReferringProviderEntityIdfr] VARCHAR(3) NULL,
	[NM103_ReferringProviderLastName] VARCHAR(60) NULL,
	[NM104_ReferringProviderFirst] VARCHAR(35) NULL,
	[NM105_ReferringProviderMiddle] VARCHAR(25) NULL,
	[NM107_ReferringProviderSuffix] VARCHAR(10) NULL,
	[NM109_ReferringProviderID] VARCHAR(80) NULL,

	[REF01_ReferringProviderSecondaryIdQlfr] VARCHAR(3) NULL,
	[REF02_ReferringProviderSecondaryID] VARCHAR(50) NULL,
	[REF04_02_ReferngProvdrSecndryPayrIdNo] VARCHAR(50) NULL,

	[N301_AmbulancePickupAddress1] VARCHAR(55) NULL,
	[N302_AmbulancePickupAddress2]  VARCHAR(55) NULL,

	[N401_AmbulancePickupCity] VARCHAR(30) NULL,
	[N402_AmbulancePickupState] VARCHAR(2) NULL,
	[N403_AmbulancePickupZip] VARCHAR(15) NULL,
	[N404_AmbulancePickupCountryCode] VARCHAR(3) NULL,
	[N407_AmbulncePickupCntrySubdivisn] VARCHAR(3) NULL,

	[NM103_AmbulanceDropOffName] VARCHAR(60) NULL,

	[N301_AmbulanceDropOffAddress1] VARCHAR(55) NULL,
	[N302_AmbulanceDropOffAddress2] VARCHAR(55) NULL,

	[N401_AmbulanceDropOffCity] VARCHAR(30) NULL,
	[N402_AmbulanceDropOffState] VARCHAR(2) NULL,
	[N403_AmbulanceDropOffZip] VARCHAR(15) NULL,
	[N404_AmbulanceDropOffCountryCode] VARCHAR(3) NULL,
	[N407_AmbulnceDropOffCntrySubdivisn] VARCHAR(3) NULL,

	CONSTRAINT PK_837X222_ServiceLineIndex PRIMARY KEY(ServiceLinekey),

	CONSTRAINT FK_837X222_ServiceLineInterReference FOREIGN KEY(Claimskey)
		 REFERENCES [837X222_Claims](Claimskey)

)

CREATE TABLE [837X222_ServiceLineAdj]
(
	[ServiceLineAdjkey] INT IDENTITY(1,1) NOT NULL,
	[ServiceLinekey] int NULL,

	[SVD01_OtherPayerPrimaryIdfr] VARCHAR(80)  NULL,
	[SVD02_ServiceLinePaidAmount] VARCHAR(18)  NULL,
	[SVD03_01_ProductServiceIdQlfr] VARCHAR(2)  NULL,
	[SVD03_02_ProcedureCode] VARCHAR(48)  NULL,
	[SVD03_03_ProcedureModifier1] VARCHAR(2) NULL,
	[SVD03_04_ProcedurModifier2] VARCHAR(2) NULL,
	[SVD03_05_ProcedureModifier3] VARCHAR(2) NULL,
	[SVD03_06_ProcedureModifier4] VARCHAR(2) NULL,
	[SVD03_07_ProcedureCodeDesc] VARCHAR(80) NULL,
	[SVD05_PaidServiceUnitCount] VARCHAR(15)  NULL,
	[SVD06_LineNumber] VARCHAR(6) NULL,

	[CAS01_AdjustmentReasonCode] VARCHAR(2)  NULL,
	[CAS02_AdjustmentAmount] VARCHAR(5)  NULL,
	[CAS03_AdjustmentQuantity] VARCHAR(18)  NULL,
	[CAS04_Quantity] VARCHAR(15) NULL,
	[CAS05_AdjustmentReasonCode2] VARCHAR(5)  NULL,
	[CAS06_AdjustmentAmount2] VARCHAR(18)  NULL,
	[CAS07_AdjustmentQuantity2] VARCHAR(15)  NULL,
	[CAS08_AdjustmentReasonCode3] VARCHAR(5)  NULL,
	[CAS09_AdjustmentAmount3] VARCHAR(18)  NULL,
	[CAS10_AdjustmentQuantity3] VARCHAR(15)  NULL,
	[CAS11_AdjustmentReasonCode4] VARCHAR(5)  NULL,
	[CAS12_AdjustmentAmount4] VARCHAR(18)  NULL,
	[CAS13_AdjustmentQuantity4] VARCHAR(15)  NULL,
	[CAS14_AdjustmentReasonCode5] VARCHAR(5)  NULL,
	[CAS15_AdjustmentAmount5] VARCHAR(18)  NULL,
	[CAS16_AdjustmentQuantity5] VARCHAR(15)  NULL,
	[CAS17_AdjustmentReasonCode6] VARCHAR(5)  NULL,
	[CAS18_AdjustmentAmount6] VARCHAR(18)  NULL,
	[CAS19_AdjustmentQuantity6] VARCHAR(15)  NULL,

	[DTP03_AdjudicationPaymentDate] VARCHAR(35)  NULL,

	[AMT02_RemainingPatientLiability] VARCHAR(18)  NULL,

	CONSTRAINT PK_837X222_ServiceLineAdjIndex PRIMARY KEY(ServiceLineAdjkey),

	CONSTRAINT FK_837X222_ServiceLineAdjInterReference FOREIGN KEY(ServiceLinekey)
		 REFERENCES [837X222_ServiceLine](ServiceLinekey)

)

CREATE TABLE [837X222_ServiceDocument]
(

	[ServiceDocumentkey] INT IDENTITY(1,1) NOT NULL,
	[ServiceLinekey] int NULL,

	[LQ01_CodeListQualifierCode] VARCHAR(3) NULL,
	[LQ02_FormIdentifier] VARCHAR(30) NULL,

	[FRM01_QuestionNumberLetter] VARCHAR(20) NULL,
	[FRM02_QuestionResponseCode] VARCHAR(1) NULL,
	[FRM03_QuestionResponse] VARCHAR(50) NULL,
	[FRM04_QuestionResponseDate] VARCHAR(8) NULL,
	[FRM05_QuestionResponsePercent] VARCHAR(6) NULL,

	CONSTRAINT PK_837X222_ServiceDocument PRIMARY KEY(ServiceDocumentkey),

	CONSTRAINT FK_837X222_ServiceDocument FOREIGN KEY(ServiceLinekey)
		 REFERENCES [837X222_ServiceLine](ServiceLinekey)

)

GO