# golf-scoreboard

# About
An API based on data scraped from http://www.espn.com/golf/leaderboard (e.g. The Masters, but will work for any competition on a given week). Built using a web API with minimal API, ASP.NET Core, .NET 6 and docker compose support.

Based on https://github.com/loisaidasam/the-masters-api

# Usage
Run using `docker-compose up` from a Terminal. Then configure [Nginx Proxy Manager](https://github.com/NginxProxyManager) to bind the Web API docker container to a DNS record.
Note: I am not sharing my UI, but it is a static website that I include in my docker compose and is therefore accessible by Nginx Proxy Manager.

![Proxy Host Configuration for your UI](https://user-images.githubusercontent.com/5255084/162196260-47fe4eb2-5e56-45f7-ac35-252b8f0f2cef.png)

![API as a Path under the UI's configuration, to avoid CORS errors](https://user-images.githubusercontent.com/5255084/162196263-93607daa-3494-41a7-a2b5-8e700bcfbbda.png)

![SSL configuration using LetsEncrypt](https://user-images.githubusercontent.com/5255084/162196267-2964821b-a070-4ed6-8672-944aca7d2d54.png)

# Example
https://masters.bigquizross.com/api/
