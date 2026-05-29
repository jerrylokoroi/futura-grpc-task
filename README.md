# futura-grpc-task

## Prerequisites
- Docker Desktop
- Kind
- Helm
- kubectl

## Setup

### 1. Create Kind cluster
kind create cluster --name grpc-app --config kind-config.yaml

### 2. Install Nginx Ingress
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/main/deploy/static/provider/kind/deploy.yaml

### 3. Create namespaces
kubectl create namespace customer1
kubectl create namespace customer2

### 4. Create secrets (required before deploying)
kubectl create secret generic db-secret --from-literal=POSTGRES_DB=ordersdb --from-literal=POSTGRES_USER=postgres --from-literal=POSTGRES_PASSWORD=postgres -n customer1

kubectl create secret generic db-secret --from-literal=POSTGRES_DB=ordersdb --from-literal=POSTGRES_USER=postgres --from-literal=POSTGRES_PASSWORD=postgres -n customer2

### 5. Deploy via Helm
helm upgrade --install order-app helm/order-app -f helm/order-app/values-customer-1.yaml -n customer1

helm upgrade --install order-app helm/order-app -f helm/order-app/values-customer-2.yaml -n customer2

### 6. Add hosts file entries (Windows)
Add the following lines to C:\Windows\System32\drivers\etc\hosts

127.0.0.1 customer1.localhost
127.0.0.1 customer2.localhost

## Access
- customer1: http://customer1.localhost:8080
- customer2: http://customer2.localhost:8080