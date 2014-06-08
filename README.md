DSS - Digital Signature Services for .NET
=========================================

This is a .NET version of European Commission "DSS - Digital Signature Services"
aimed to provide CAdES, XAdES and PAdES signature for the .NET Framework 
platform and Mono.

It uses some open source libraries written in C#:

###iTextsharp v5.4.4:
- BouncyCastle (CAdES signature and crypto utilities)
- PDF Signature (not integrated yet)

https://github.com/itext/itextsharp

###Microsoft.Xades v1.0.0:
- XAdES signature (XAdES 1.1.1 tried to be extended to 1.3.2)

http://www.microsoft.com/france/openness/open-source/interoperabilite_xades.aspx

###DSS v2.0.2:
- Java version ported to C# from the original European Commission DSS, using the 
Sharpen version included in NGIT by "slluis". Only ported "dss-document", 
"dss-service" and "dss-spi". 
Note this is mostly a manual port with the help of Sharpen.

https://joinup.ec.europa.eu/asset/sd-dss/description
https://github.com/mono/ngit

###About this release:
- Succesfully signs and verifies CAdES-BES, CAdES-T, CAdES-C, CAdES-X and CAdES-XL.
- First attempt to sign XAdES-BES, XAdES-T, XAdES-C, XAdES-X and XAdES-XL.

###TODO:
- Fix XAdES signature. It fails with some verifiers.
- Implement XAdES verification.
- Integrate PAdES signature and verification.
- Port Microsoft.Xades to Mono.

###Disclaimer
I am not affiliated in any way with the European Commission nor the original DSS project. This is an independent port to C#.
