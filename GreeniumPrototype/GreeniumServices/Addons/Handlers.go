package main

import (
	"fmt"
	"net/http"
	"time"
)

// Services
func Addons(w http.ResponseWriter, r *http.Request) {

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

	//vars := mux.Vars(r)

	w.Header().Set("Content-Type", "application/json; charset=UTF-8")
	w.WriteHeader(http.StatusOK)

}

func Download(w http.ResponseWriter, r *http.Request) {

	w.Header().Set("Content-Type", "application/json; charset=UTF-8")
	w.WriteHeader(http.StatusOK)

}
