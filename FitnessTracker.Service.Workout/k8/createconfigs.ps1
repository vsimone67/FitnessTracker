kubectl delete secret appsettings-secret-workoutservice --namespace fitnesstracker
 
kubectl delete configmap appsettings-workoutservice --namespace fitnesstracker

kubectl create secret generic appsettings-secret-workoutservice --namespace fitnesstracker --from-file=../../configfiles/workout/appsettings.secrets.json

kubectl create configmap appsettings-workoutservice --namespace fitnesstracker --from-file=../../configfiles/workout/appsettings.json