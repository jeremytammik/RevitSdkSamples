;;
;; acad.lsp - load RstLink AutoCAD client app 
;;
;; Copyright (C) 2008 by Jeremy Tammik, Autodesk Inc., 2008-04-30
;;
;; (load "C:/a/j/adn/train/revit/2009/src/rst/link/RSLinkAcadClient/acad.lsp")
;;
(defun s::startup()
  (command "_netload" "C:/a/j/adn/train/revit/2009/src/rst/link/RSLinkAcadClient/bin/RSLinkAcadClient.dll")
  (princ "\nAutoCAD RSLink client loaded.")
  (arxload "C:/a/j/adn/train/revit/2009/src/rst/link/RSLinkAcadClientDynProps/Debug/RSLinkAcadClientDynProps.arx")
  (princ "\nAutoCAD RSLink dynamic properties loaded.")
  (princ)
)
