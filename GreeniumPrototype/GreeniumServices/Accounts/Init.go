package main

import (
	"fmt"
	"log"
	"net/http"
	"os"
	"strconv"
	// Git repos here
)

var redisConnector *RedisClientWrapper

func main() {
	address := "localhost:6379" // Automatically redirect to the dev database if no inputs
	password := ""
	dbIndex := 0

	if (len(os.Args) - 1) > 3 {
		address = os.Args[1]
		password = os.Args[2]
		dbIndexTemp, err := strconv.Atoi(os.Args[3])

		if err != nil {
			fmt.Printf("%s could not be parsed", os.Args[3])
			dbIndex = 0
		} else {
			dbIndex = dbIndexTemp
		}
	}

	redisConnector = NewConnection(address, password, dbIndex)

	log.Printf("Opening the database %s %d", address, dbIndex)
	log.Println("")

	router := NewRouter()
	log.Fatal(http.ListenAndServe(":10840", router))

}
