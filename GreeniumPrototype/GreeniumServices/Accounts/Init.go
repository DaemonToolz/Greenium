package main

import (
	"log"
	"net/http"
	// Git repos here
)

func main() {

	router := NewRouter()
	log.Fatal(http.ListenAndServe(":10840", router))

}
