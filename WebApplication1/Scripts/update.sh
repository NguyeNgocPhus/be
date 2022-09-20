#!/usr/bin/env bash
dotnet ef database update --project Cart.Infrastructure --startup-project Cart.WebApi --context ApplicationDbContext