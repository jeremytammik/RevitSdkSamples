;;
;; acad.lsp - load RstLink AutoCAD client app 
;;
;; Copyright (C) 2008 by Jeremy Tammik, Autodesk Inc., 2008-04-30
;;
;; (load "C:/a/j/adn/train/revit/2009/src/rst/link/vb/RstLinkAcadClient/acad.lsp")
;;
(defun s::startup()
  (command "_netload" "C:/a/j/adn/train/revit/2009/src/rst/link/vb/RstLinkAcadClientVb/bin/RstLinkAcadClientVb.dll")
  (princ "\nAutoCAD RstLink client loaded.")
  (arxload "C:/a/j/adn/train/revit/2009/src/rst/link/cpp/RstLinkAcadClientDynProps/Debug/RstLinkAcadClientDynProps.arx")
  (princ "\nAutoCAD RstLink dynamic properties loaded.")
  (princ)
)
