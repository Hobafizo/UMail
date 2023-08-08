# UMail  _(Work In Progress)_

Multi-functional SMTP client that can handle organizational emails and send mails based on departments.

<br>
<p align="center">
  <img src="https://cdn-icons-png.flaticon.com/512/5578/5578703.png" height="300">
</p>
<br>

## Dependencies
- **.NET 7.0**
- **Newtonsoft.Json (13.0.3)**

## Features
- [x] Flexible configuration that supports commenting. _(begin line with //)_
- [ ] Asynchronous base that can possibly support multithreading.
- [x] Deals with multiple SMTP servers.
- [x] Handles multiple emails and organization departments.
- [x] Embed images in email body.
- [x] Send HTML body in emails.
- [ ] Receive email requests through a TCP server, can commit plenty of requests from different connections at a time.
- [ ] Receive email requests using MSSQL.
- [ ] Generate reports of email activities based on department.
