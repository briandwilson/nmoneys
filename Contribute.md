# How can I contribute? #

Many people can do many things for NMoneys. Indeed, one does not have to be a .NET developer to contribute to the project.

## I am a .NET developer ##
In that case you are more than welcome to sneak around the code and find issues. Of course, everything is more fun if you submit patches to solve those issues, but letting us know what the problem is will get everyone a long way.
You might as well have nice features that you want to get implemented. In that case you have basically two options:
  * Ask for them and wait to be implemented (if we have the knowledge to implement them at all)
  * Roll up your sleeves an contribute actively to the project, which will surely speed up the process.

As the fellow guys from [Moq](http://code.google.com/p/moq/) I like the approach outlined by [Instiki](http://www.instiki.org/show/HowToContribute) to send corrections. Use our [trunk](https://nmoneys.googlecode.com/svn/trunk/) version of the repository.

## I am not a .NET developer ##
As we mentioned you need not to be a developer to contribute. The main source of information is an almost-human-readable XML file that contains the configuration for each and every of the currencies in the ISO 4217 Standard. Native names can be misspelled, symbols might be wrong, number of significant digits might be less or more, etc.
As a matter of fact, the correctness and completeness of that XML is what will make a difference for the success of the library, so people that know about localization and formatting of "exotic" currencies are highly encouraged to contribute.