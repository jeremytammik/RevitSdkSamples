﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.3053
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On



'''
<Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Applications.ProgrammingModel.dll", "9.0.0.0"),  _
 Global.System.CLSCompliantAttribute(false)>  _
Partial Public NotInheritable Class ThisApplication
    Inherits Autodesk.Revit.ApplicationEntryPoint
    Implements Microsoft.VisualStudio.Tools.Applications.Runtime.IEntryPoint
    
    Public Event Startup As System.EventHandler
    
    Public Event Shutdown As System.EventHandler
    
    '''
    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Never)>  _
    Private Sub OnStartup()
    End Sub
    
    '''
    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Never)>  _
    Protected Overrides Sub FinishInitialization()
        MyBase.FinishInitialization
        Me.OnStartup
        If (Not (Me.StartupEvent) Is Nothing) Then
            RaiseEvent Startup(Me, System.EventArgs.Empty)
        End If
    End Sub
    
    '''
    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Never)>  _
    Protected Overrides Sub OnShutdown()
        If (Not (Me.ShutdownEvent) Is Nothing) Then
            RaiseEvent Shutdown(Me, System.EventArgs.Empty)
        End If
        MyBase.OnShutdown
    End Sub
    
    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Never)>  _
    Protected Overrides ReadOnly Property PrimaryCookie() As String
        Get
            Return "ThisApplication"
        End Get
    End Property
End Class