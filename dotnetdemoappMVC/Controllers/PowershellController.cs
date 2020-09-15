using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Management.Automation;
using System.Text;

namespace dotnetdemoappMVC.Controllers
{
    public class PowershellController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
       public ActionResult preFlightChecks()
        {
            
            var powerShell = PowerShell.Create();
            var preReqInlineScript = "" +
                "$checksPassed = $null;" +
                "$checkADPowershell = (Get-Command Test-ADServiceAccount).ModuleName;" +
                "$iisDocRoot = \"C:\\inetpub\\wwwroot\\\";" +
                "$powerShellScriptRoot = \"C:\\inetpub\\wwwroot\\Powershell\\\";" +
                "If ($checkADPowershell -eq 'ActiveDirectory') " +
                    "{Write-Output \"[PASS]  Active Directory RSAT Powershell Module Installed`n\"; " +
                    "If ($checksPassed -eq $null) {$checksPassed = $TRUE;};} " +
                "Else " +
                    "{Write-Output \"[FAIL]  Active Directory RSAT Powershell Module Missing\";" +
                    "Write-Output \"        Please add dependency in Dockerfile or install the tools interactively on the Container`n\";" +
                    "Write-Output \"        Dockerfile: RUN Add-WindowsFeature RSAT-AD-Powershell\";" +
                    "If ($checksPassed -eq $null) {$checksPassed = $FALSE;}};" +
                "$checkIISDocRoot = Test-Path $iisDocRoot;" +
                "If ($checkIISDocRoot -eq $TRUE)" +
                    "{Write-Output \"[PASS]  IIS Document Root found\";" +
                    "Write-Output \"        $iisDocRoot`n\";" +
                    "If ($checksPassed -eq $TRUE) {$checksPassed = $TRUE;}}" +
                "Else " +
                    "{Write-Output \"[FAIL]  IIS Document Root is not correct\";" +
                    "Write-Output \"        Please ensure that all WebApp Files are in the $iisDocRoot Folder`n\";" +
                    "$checksPassed = $FALSE;};" +
                "$checkPowershellRoot = Test-Path $powerShellScriptRoot;" +
                "If ($checkPowershellRoot -eq $TRUE) " +
                    "{Write-Output \"[PASS]  Powershell Scripts Folder found\";" +
                    "Write-Output \"        $powerShellScriptRoot`n\";" +
                    "If ($checksPassed -eq $TRUE) {$checksPassed = $TRUE;};} " +
                "Else " +
                    "{Write-Output \"[FAIL]  Powershell Scripts Folder not found\";" +
                    "Write-Output \"        Please ensure that all Powershell Scripts are in the $powerShellScriptRoot Folder`n\";" +
                    "$checksPassed = $FALSE;};" +
                "$checkcontainerPS = Test-Path \"$powerShellScriptRoot\\containerDiag.ps1\";" +
                "If ($checkcontainerPS -eq $TRUE) " +
                    "{Write-Output \"[PASS]  Container Diagnostic Script found\";" +
                    "Write-Output \"        $powerShellScriptRoot\\containerDiag.ps1`n\";" +
                    "If ($checksPassed -eq $TRUE) {$checksPassed = $TRUE;};}" +
                "Else " +
                    "{Write-Output \"[FAIL]  Container Diagnostic Script not found\";" +
                    "Write-Output \"        Please ensure that all Powershell Scripts are in the $powerShellScriptRoot Folder`n\";" +
                    "$checksPassed = $FALSE;};" +
                "$checkdomainPS = Test-Path \"$powerShellScriptRoot\\domainDiag.ps1\";" +
                "If ($checkdomainPS -eq $TRUE) " +
                    "{Write-Output \"[PASS]  Domain Diagnostic Script found\";" +
                    "Write-Output \"        $powerShellScriptRoot\\domainDiag.ps1`n\";" +
                    "If ($checksPassed -eq $TRUE) {$checksPassed = $TRUE;};}" +
                "Else" +
                    "{Write-Output \"[FAIL]  Domain Diagnostic Script not found\";" +
                    "Write-Output \"        Please ensure that all Powershell Scripts are in the $powerShellScriptRoot Folder`n\";" +
                    "$checksPassed = $FALSE;};" +
                "If ($checksPassed -eq $TRUE) " +
                    "{Write-Output \"[RES]   Result: PASS   All checks passed! Please proceed to run the different tests.\";}" +
                "Else" +
                    "{Write-Output \"[RES]   Result: FAIL   One of more prerequisite checks failed. Please fix the issues and re-run the checks before proceeding\";}";
            powerShell.Commands.AddScript(preReqInlineScript);
            var psOutput = powerShell.Invoke();

            if (psOutput.Count > 0)
            {
                var strBuild = new StringBuilder();

                foreach (var psObject in psOutput)
                {
                    strBuild.Append(psObject.BaseObject.ToString() + "\r\n");
                }
                return Content(strBuild.ToString());
            }

            return Json("");
        }
        public ActionResult containerDiagnostics()
        {

            var powerShell = PowerShell.Create();
            powerShell.Commands.AddScript("C:\\inetpub\\wwwroot\\Powershell\\containerDiag.ps1");
            var psOutput = powerShell.Invoke();

            if (psOutput.Count > 0)
            {
                var strBuild = new StringBuilder();

                foreach (var psObject in psOutput)
                {
                    strBuild.Append(psObject.BaseObject.ToString() + "\r\n");
                }
                return Content(strBuild.ToString());
            }

            return Json("");
        }
        public ActionResult domainDiagnostics(string gMSAInput)
        {

            var powerShell = PowerShell.Create();
            powerShell.Commands.AddScript("C:\\inetpub\\wwwroot\\Powershell\\domainDiag.ps1 " + gMSAInput);
            var psOutput = powerShell.Invoke();

            if (psOutput.Count > 0)
            {
                var strBuild = new StringBuilder();

                foreach (var psObject in psOutput)
                {
                    strBuild.Append(psObject.BaseObject.ToString() + "\r\n");
                }
                return Content(strBuild.ToString());
            }

            return Json("");
        }

    }
}