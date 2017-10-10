package main

import (
	"encoding/json"
	"fmt"
	"net/http"
	"time"

	"github.com/gorilla/mux"
)

// Services
func LoadAccount(w http.ResponseWriter, r *http.Request) {

	fmt.Println("[%s] - Request from %s ", time.Now().Format(time.RFC3339), r.RemoteAddr)

	// Check unauthorized. Replace this Authorization token by a valid one
	// by automatic generation and / or a new and dedicated web service
	/*
		if r.Header.Get("Token") != "Jkd855c6x9Aqcf" {
			w.Header().Set("Content-Type", "application/json; charset=UTF-8")
			w.WriteHeader(http.StatusForbidden)
			panic("Non authorized access detected")
		}
	*/

	vars := mux.Vars(r)
	AccountID := vars["AccountID"]

	aChannel := make(chan AccountModel)
	defer close(aChannel)

	go Find(AccountID, aChannel)

	w.Header().Set("Content-Type", "application/json; charset=UTF-8")
	w.WriteHeader(http.StatusOK)

	if err := json.NewEncoder(w).Encode(<-aChannel); err != nil {
		panic(err)
	}

}

func CreateAccount(w http.ResponseWriter, r *http.Request) {
	defer r.Body.Close()
	decoder := json.NewDecoder(r.Body)

	var post AccountCreateRequest
	err := decoder.Decode(&post)

	if err != nil {
		panic(err)
	}

	cChannel := make(chan AccountModel)

	go Create(post.Name, post.FullName, cChannel)

	w.Header().Set("Content-Type", "application/json; charset=UTF-8")
	w.WriteHeader(http.StatusOK)

	if err := json.NewEncoder(w).Encode(<-cChannel); err != nil {
		panic(err)
	}

}
