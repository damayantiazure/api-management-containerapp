echo "Starting deploying the app provisioning..."

docker build -t <REG NAME>/<IMAGE NAME>:$tagName -f src/APIControlPanel/Dockerfile .
docker push <REG NAME>/<IMAGE NAME>:$tagName
