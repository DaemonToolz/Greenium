package main

import (
	"log"
	"net/http"
	// Git repos here
)

var redisConnector = NewConnection("localhost:6379", "", 0)

func main() {

	router := NewRouter()
	log.Fatal(http.ListenAndServe(":10840", router))

}
