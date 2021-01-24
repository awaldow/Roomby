az keyvault list-deleted --subscription d2591db1-df2a-42f6-ac46-90c8d14a6418 --resource-type vault

az keyvault purge --subscription d2591db1-df2a-42f6-ac46-90c8d14a6418 -n kv-roomby-rooms-test
az keyvault purge --subscription d2591db1-df2a-42f6-ac46-90c8d14a6418 -n kv-roomby-users-test