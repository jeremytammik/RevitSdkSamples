;;
;; acad.lsp - load RstLink AutoCAD client app 
;;
;; Copyright (C) 2008-2010 by Jeremy Tammik, Autodesk Inc. All rights reserved.
;;
;; (load "C:/a/j/adn/train/revit/2011/src/rst/link/cs/RstLinkAcadClientCs/bin/acad.lsp")
;;
(defun s::startup()
  (setq root "C:/a/j/adn/train/revit/2011/src/rst/link")
  (command "_netload" (strcat root "/cs/RstLinkAcadClientCs/bin/RstLinkAcadClient.dll"))
  (princ "\nAutoCAD RstLink client loaded.")
  (arxload (strcat root "/cpp/RstLinkAcadClientDynProps/Debug/RstLinkAcadClientDynProps.arx"))
  (princ "\nAutoCAD RstLink dynamic properties loaded.")
  (princ)
)
