# Setup instructions for server

Assumptions:

- Running on a DigitalOcean Ubuntu droplet
- Using supervisorctl for managing the process
- All is running on `dimo` user

## Dependencies

- .NET v7+
- PostgresDB on port 5432
- RabbitMQ on port 5672

## .NET

```shell
curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel STS
```

## Postgres

### Install

```shell
sudo apt-get update
sudo apt-get install postgresql postgresql-contrib
sudo systemctl start postgresql
sudo systemctl enable postgresql
```

### Create user

```shell
sudo su postgres
psql
```

```postgresql
\password
<newpassword>
<newpassword>
\q
```

```shell
exit
```

### Create database

```shell
psql -h 127.0.0.1 -U postgres
```

```postgresql
CREATE DATABASE "vehicle-genius";
\q
```

## RabbitMQ

```shell
sudo apt-get update
sudo apt-get install rabbitmq-server
sudo systemctl start rabbitmq-server
sudo systemctl enable rabbitmq-server
```

## supervisorctl

```shell
sudo apt-get update
sudo apt-get install supervisor
cp supervisorctl.conf /etc/supervisor/conf.d/vehicle-genius-api.conf
```
