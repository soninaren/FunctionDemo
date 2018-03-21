# FunctionDemo

This repo shows an error when using Azure Functions that reference an external NetStandard assembly that references Microsoft.Extensions.Logging.

The error received is:

[3/21/2018 5:34:33 PM] ScriptHost initialization failed

[3/21/2018 5:34:33 PM] System.Private.CoreLib: Could not load file or assembly 'Microsoft.Extensions.Logging.Abstractions, Version=2.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'. Could not find or load a specific file. (Exception from HRESULT: 0x80131621). System.Private.CoreLib: Could not load file or assembly 'Microsoft.Extensions.Logging.Abstractions, Version=2.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'.
