#pragma once

class SampleClipAngle : 
   public IDispatchImpl<ISampleClipAngle, &IID_ISampleClipAngle, &LIBID_SampleClipJointLib>,
   public ISupportErrorInfo,
   public CComObjectRoot,
   public CComCoClass<SampleClipAngle, &CLSID_SampleClipAngle>,
   public IDispatchImpl<IRule, &IID_IRule, &LIBID_AstSTEELAUTOMATIONLib, MAJVER_AstSTEELAUTOMATIONLib>,
   public IDispatchImpl<IRuleExtension, &IID_IRuleExtension, &LIBID_AstSTEELAUTOMATIONLib, MAJVER_AstSTEELAUTOMATIONLib>
{

private:
   IJoint* m_pJoint;
   HRESULT MovePoint(IPoint3dPtr ptIn, IVector3dPtr vMove, double moveDist, IPoint3dPtr& retPoint);

   HRESULT CreateBoltPattern(CString boltRole, double dBoltWidth, double dBoltHeight, int nXBolts, int nYBolts, ICS3dPtr csBolts, IBeamPtr cleat1, IBeamPtr cleat2, IBeamPtr beam, IAstObjectsArrPtr& createdObjectsArr);

   HRESULT IntersectBeam(IStraightBeamPtr beam, IPoint3dPtr linePt, IVector3dPtr lineVect, IPoint3dArrayPtr &ptsIntersection);
   
   HRESULT GetBeamCSAtPoint(IBeamPtr spBeam, IPoint3dPtr spPoint, ICS3dPtr& spCS);
   
   HRESULT GetFacePlane(IStraightBeamPtr inputColumn, IStraightBeamPtr inputBeam, IPlanePtr &resPlane);

   void SplitBeamsInternalName(BSTR fullInternal, BSTR& classInternal, BSTR& sectionInternal);

public:
   BEGIN_COM_MAP(SampleClipAngle)
      //DEL 	COM_INTERFACE_ENTRY(IDispatch)
      COM_INTERFACE_ENTRY(ISampleClipAngle)
      COM_INTERFACE_ENTRY(ISupportErrorInfo)
      COM_INTERFACE_ENTRY2(IDispatch, ISampleClipAngle)
      COM_INTERFACE_ENTRY(IRule)
      COM_INTERFACE_ENTRY(IRuleExtension)
   END_COM_MAP()
   //DECLARE_NOT_AGGREGATABLE(CUSClipAngle) 
   // Remove the comment from the line above if you don't want your object to 
   // support aggregation. 

   DECLARE_REGISTRY_RESOURCEID(IDR_SampleClipAngle)
   // ISupportsErrorInfo
   STDMETHOD(InterfaceSupportsErrorInfo)(REFIID riid);


   // IRule
   STDMETHOD(get_Joint)(IJoint** pVal);
   STDMETHOD(put_Joint)(IJoint* pVal);
   STDMETHOD(Query)(IAstUI* pAstUI);
   STDMETHOD(InField)(IFiler* pFiler);
   STDMETHOD(OutField)(IFiler* pFiler);
   STDMETHOD(GetTableName)(BSTR* pStrTableName);
   STDMETHOD(CreateObjects)();
   STDMETHOD(GetUserPages)(IRulePageArray* pagesRet, IPropertySheetData* pPropSheetData);
   STDMETHOD(FreeUserPages)();
   STDMETHOD(GetExportData)(IRuleExportFiler* pExportFiler);
   STDMETHOD(GetFeatureName)(BSTR* FeatureName, VARIANT_BOOL* hasFeature);
   STDMETHOD(InvalidFeature)(INT reserved);
   STDMETHOD(ConvertFromHRL)(IConvertFiler* filer, BSTR OldHRLRuleName, VARIANT_BOOL* Converted);
   STDMETHOD(BoldParameters)(SAFEARRAY**);

   class NativeClipAnglePage* m_page;
   BSTR m_sProfType;
   BSTR m_sProfSize;
   BSTR m_sBoltsStandard;
   BSTR m_sBoltsMaterial;
   BSTR m_sBoltsSet;
   double m_dBoltsDiameter;
};