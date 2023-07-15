
all: publish run

publish: 
	dotnet publish

run:
	./bin/Debug/net7.0-windows/publish/puka.exe

