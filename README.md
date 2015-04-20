# GLAM-Hashavshevet-service
a POC of a web service that offers sale agents web interface to push their orders into hashavshevet.
this is done via server that runs the actual program and automaticly types the order in.

Glam-Server contains also the static web files (JS, CSS, Html) and the Restless service that recieves orders (JSON format)
and queues them up so a background process can pick them up one at a time and run the Hashavshevet adapter to process them.

GlamHashAdapter is the background services that starts Hashavshevet, picks up an order from the queue and automaticly type it into the orders screen in Hashavshevet.
this is done via TestStack.White automation framework (https://github.com/TestStack/White)

Technologies (as far as i can remember):

ASP.NET MVC+Web API 4

TestStack.White automation framework


Prerequesites:

Hashavshevet - you can download a student hashavshevet from their web site.
MSSQL
generated DB via scripts

DB generation script is in: GlamHashAdapter/GlamHashAdapter/DBScripts
DB generation with demo data is available

Note that this is a POC only and might not work on any screen since the TestStack.White automation framework uses mouse cordinations sometimes to focus on the right text fields in the screen.
Use it on your own risk :)

Here's a video of how it works:
https://youtu.be/UvD-r4LkbF0

[![GLAM presentation (hebrew)](http://img.youtube.com/vi/UvD-r4LkbF0/0.jpg)](https://youtu.be/UvD-r4LkbF0)


