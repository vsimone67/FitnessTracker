kubectl delete secret appsettings-secret-dietservice --namespace fitnesstracker

kubectl delete configmap appsettings-dietservice --namespace fitnesstracker

kubectl create secret generic appsettings-secret-dietservice --namespace fitnesstracker --from-file=../../configfiles/diet/appsettings.secrets.json

kubectl create configmap appsettings-dietservice --namespace fitnesstracker --from-file=../../configfiles/diet/appsettings.json