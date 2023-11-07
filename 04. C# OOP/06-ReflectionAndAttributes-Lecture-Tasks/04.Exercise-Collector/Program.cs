//4 Exercise - Collector
using Stealer;

Spy spy = new Spy();
//spy.StealFieldInfo("Stealer.Hacker", "username", "password");
//spy.AnalyzeAccessModifiers("Stealer.Hacker");
//spy.RevealPrivateMethods("Stealer.Hacker");
spy.CollectGetterAndSetters("Stealer.Hacker");