package main

import (
	"encoding/json"
	"fmt"
	"log"
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
	AccountID := vars["ID"]

	log.Printf("Accessing account %s ", AccountID)

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
	defer close(cChannel)

	go Create(post.Name, post.FullName, post.Emails, post.UID, cChannel)

	w.Header().Set("Content-Type", "application/json; charset=UTF-8")
	w.WriteHeader(http.StatusOK)

	if err := json.NewEncoder(w).Encode(<-cChannel); err != nil {
		panic(err)
	}

}

func GainXP(w http.ResponseWriter, r *http.Request) {
	defer r.Body.Close()
	decoder := json.NewDecoder(r.Body)

	var post AccountCreateRequest
	err := decoder.Decode(&post)

	if err != nil {
		panic(err)
	}

	cChannel := make(chan bool)
	defer close(cChannel)

	go AddXP(post.ID, post.XpGain, cChannel)

	w.Header().Set("Content-Type", "application/json; charset=UTF-8")
	w.WriteHeader(http.StatusOK)

	if err := json.NewEncoder(w).Encode(<-cChannel); err != nil {
		panic(err)
	}

}

func SetEmails(w http.ResponseWriter, r *http.Request) {
	defer r.Body.Close()
	decoder := json.NewDecoder(r.Body)

	var post AccountCreateRequest
	err := decoder.Decode(&post)

	if err != nil {
		panic(err)
	}

	cChannel := make(chan bool)
	defer close(cChannel)

	go UpdateEmails(post.ID, post.Emails, cChannel)

	w.Header().Set("Content-Type", "application/json; charset=UTF-8")
	w.WriteHeader(http.StatusOK)

	if err := json.NewEncoder(w).Encode(<-cChannel); err != nil {
		panic(err)
	}

}

func Login(w http.ResponseWriter, r *http.Request) {
	defer r.Body.Close()
	decoder := json.NewDecoder(r.Body)

	var post LoginRequest
	err := decoder.Decode(&post)

	if err != nil {
		panic(err)
	}

	cChannel := make(chan AccountModel)
	defer close(cChannel)

	go FindId(post.ID, cChannel)

	w.Header().Set("Content-Type", "application/json; charset=UTF-8")
	w.WriteHeader(http.StatusOK)

	if err := json.NewEncoder(w).Encode(<-cChannel); err != nil {
		panic(err)
	}

}
