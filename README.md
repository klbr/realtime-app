# Realtime Process Monitor - Client/Server
Application developed with .NETCore (SignalR) with display function all processes on multiples clients instances. Vuejs frontend to show status to system admins.

## Status.Realtime.Domain
Domain object shared between Client and Server

## Status.Realtime.ServerHub
Server for Api and Hub (SignalR)
Persistence with LiteDB (Embedded NoSQL Database)

## Status.Realtime.Client
Client side to collect processes and send to server

## Status.Realtime.Front
Frontend for Server Application 


> TODO:
> * Send command to Turn off and on clients notification
> * Improve Frontend usability and face friendly
> * Show processes history based on last 24 hours
> * Easy install on clients instances

