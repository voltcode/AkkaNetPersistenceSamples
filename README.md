# AkkaNetPersistenceSamples
This repository hosts code sample for the "Event Sourcing and beyond  with Akka.NET Persistence" presentation that demonstrates core features of the Akka.NET Persistence module.

Akka.NET is a port of Akka framework on JVM that implements the Actor Model and provides many utilities for building highly available and robust systems.

Akka.NET Persistence is a module of the Akka.NET framework that adds event sourcing capabilities and at-least-once delivery method for communication between actors.

It is recommended to browse my presentation first before going through the sample https://www.slideshare.net/KonradDusza1/event-sourcing-and-beyond-with-akkanet-persistence-119797422

Slide 16 contains a diagram depicting core message flow in the sample.

## About the sample

The sample illustrates the usefulness of Akka.NET Persistence using message exchange between a cash machine and a bank.

The sample consists of two main actors: 
* CashMachine representing a classic ATM that needs to talk to a bank sometimes
* Banker representing a bank keeping track of its' customer accounts

For simplicity's sake, sample assumes only 1 user of the system, ie. the bank stores the account state only for a single user.

### CashMachine 

Project: Voltcode.AkkaNetPersistenceSamples.Accounting.CashMachine
There are two main parts in this project:
* WinForms GUI form, that imitate a ATM GUI. User can: credit account, debit account and check his account status
* CashMachine actor that receives messages from the GUI, sends them to the Banker.

CashMachine actor, forwards the credit/debit account messages using At-Least-Once delivery capabilities to the banker. 
After all, if we paid money into the ATM, we would want that fact reflected on our account, even if there were intermittent problems with network at some point!

This actor uses sqlite for persistence. Configuration can be found in HOCON inside project's app.config.

### Banker

Project: Voltcode.AkkaNetPersistenceSamples.Accounting.Banker
Banker actor is spawned within a console application. It is a standard persistent actor that receives messages from the CashMachine, stores relevant events, and updates the account state accordingly upon successful event persistence.
Every 10 credit/debit messages, account snapshot is stored.

This actor uses MS SQL for persistence. Configuration can be found in HOCON inside project's app.config.
MS SQL database must be present for banker to run and initialized with banker_db.sql script.

## How to run?

* clone the repo
* open the solution in Visual Studio 2017
* switch to x64 solution platform
* Do a nuget restore and full rebuild
* Check persistence configuration for the banker actor - it requires a MS SQL connection. 
By default, it uses a local MS SQL Express instance, details are in app.config. 
* start CashMachine. If there are errors about missing sqlite.dll, please ensure that you executed previous steps. If problem persists, do a full rebuild again.
* start Banker

## Time to play!

Observe Akka.NET Persistence strengths by trying out several scenarios:
* Happy path - banker and cashmachine are online. Depositing money in GUI can be observed in console log of banker.
* Banker goes down - crash the banker, deposit some money in the cash machine
* start the banker again - notice how the messages will be delivered to him ! (that's at-most-delivery at play here)
* sent more than 10 deposit/credit messages - notice that banker will create a snapshot.
* restart banker and check his log for recovery messages.
* start from scratch - run both, deposit money several times. crash banker, deposit more money. crash GUI. bring back banker, bring back Cash Machine. Notice that after a while, bank will receive the money that was sent before the Cash Machine was crashed. This is recovery at play. Notice that the already delivered messages are not resent!

Notice that you didn't need a queueing system to achieve guaranteed delivery.

## Want to now more about Akka.NET?

* Check out my other repository with Akka.NET code samples: https://github.com/voltcode/AkkaNetSamples
* Visit Akka.NET home page and review Akka.NET Persistence documentation http://getakka.net/
