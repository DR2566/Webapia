#!/bin/bash

if [ -z "$1" ]; then 
    echo "migration name missing";
    exit 1;
fi;

dotnet ef migrations add "$1" --project Webapia.Infrastructure --startup-project Webapia.Api &&
dotnet ef database update --project Webapia.Infrastructure --startup-project Webapia.Api

