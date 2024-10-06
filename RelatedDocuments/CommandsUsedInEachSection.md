# Commands Used In Each Section
## Building The First Service 
```bash
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 8.1.1

dotnet add package Microsoft.EntityFrameworkCore --version 5.0.8
dotnet add package Microsoft.EntityFrameworkCore.Design --version 5.0.8
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 5.0.8
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 5.0.8
```

## Docker & Kubernetes
```bash
docker --Version
docker build --tag mohamedhashish/platformservice .
docker container run --detach --publish 8080:80 --name platformservice mohamedhashish/platformservice 
docker ps
docker stop platformservice
docker start platformservice

Kubectl version
kubectl apply --filename platforms-depl.yaml
kubectl apply --filename platforms-np-srv.yaml
kubectl get deployments
kubectl get pods
```

## Building The Second Service
```bash
dotnet new webapi --name CommandsService  --framework  net5.0

dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 8.1.1
dotnet add package Microsoft.EntityFrameworkCore --version 5.0.8
dotnet add package Microsoft.EntityFrameworkCore.Design --version 5.0.8
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 5.0.8

docker build --tag mohamedhashish/commandsservice .
docker container run --detach --publish 8080:80 --name commandsservice mohamedhashish/commandsservice 
kubectl apply --filename commands-depl.yaml
kubectl get deployments
kubectl get pods
kubectl get services
kubectl delete deployment platforms-depl  
kubectl rollout restart deployment platforms-depl.yaml

# link : https://github.com/kubernetes/ingress-nginx
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.8.1/deploy/static/provider/aws/deploy.yaml
kubectl get namespace
kubectl get pods --namespace=ingress-nginx
kubectl get services --namespace=ingress-nginx  
kubectl apply -f ingress-srv.yaml
```


## Starting With SQL Server
```bash
Kubectl get storageclass
Kubectl apply -f  local-pvc.yaml
Kubectl get pvc
Kubectl create secret generic mssql --from-literal=SA_PASSWORD="pa55w0rd!"
Kubectl apply -f mssql-plat-depl.yaml

dotnet ef migrations add InitialMigration

docker build --tag mohamedhashish/platformservice .
kubectl delete deployment platforms-depl  
kubectl apply --filename platforms-depl.yaml
kubectl describe deployment/mssql-depl
kubectl delete deployment mssql-depl
```

## Message Bus & RabbitMQ
```bash
Kubectl apply -f rabbitmq-depl.yaml
```

## Asynchronous Messaging 
```bash
dotnet add package RabbitMQ.Client --version 6.2.2

docker build --tag mohamedhashish/platformservice .
kubectl rollout restart deployment platforms-depl

docker build --tag mohamedhashish/commandsservice .
kubectl rollout restart deployment commands-depl

```

## gRPC
### Platform Service
```bash
dotnet add package Grpc.Tools --version 2.39.1
dotnet add package Grpc.Net.Client --version 2.38.0
dotnet add package Google.Protobuf --version 3.17.3
```
### Command service
```bash
dotnet add package Grpc.AspNetCore --version 2.38.0
```

 ## Deploy to Kubernetes

 ### Platform Service
```bash
dotnet build
docker build --tag mohamedhashish/platformservice .
```

### Command service
```bash
dotnet build
docker build --tag mohamedhashish/commandsservice .
```

### K8S
```bash
kubectl rollout restart deployment platforms-depl
kubectl rollout restart deployment commands-depl
```
