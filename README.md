Meerkat Mailer
==============

Provides email merge and dispatching capability for desktop and web applications.

Available as a NuGet packages [Meerkat.Mailer](https://www.nuget.org/packages/Meerkat.Mailer/)

[![NuGet](https://img.shields.io/nuget/v/Meerkat.Mailer.svg)](https://www.nuget.org/packages/Meerkat.Mailer/)
[![Build status](https://ci.appveyor.com/api/projects/status/t7wnsdotj5xj8s20/branch/master?svg=true)](https://ci.appveyor.com/project/PaulHatcher/meerkat-mailer/branch/master)

Welcome to contributions from anyone.

You can see the version history [here](RELEASE_NOTES.md).

## Build the project
* Windows: Run *build.cmd*

I have my tools in C:\Tools so I use *build.cmd Default tools=C:\Tools encoding=UTF-8*

## Library License

The library is available under the [MIT License](http://en.wikipedia.org/wiki/MIT_License), for more information see the [License file][1] in the GitHub repository.

 [1]: https://github.com/phatcher/Meerkat.Mailer/blob/master/License.md

## Getting Started

There are a number of issues to address when trying to send emails from your application...

1. Finding the appropriate template, possibly needing to take internationalization into account
2. Merging your application data over the template
3. Sending the email
4. Recording that you attempted to send the email and/or any errors in the sending process
5. Handling bounce-backs because of incorrect email addressing, out-of-office or some other issues

What Meerkat.Mailer does it to break these down into separable components so that your application can take control over some or all of them whilst still being able to use third-party solutions for dispatch e.g. sending email via SendGrid, MailGunn etc