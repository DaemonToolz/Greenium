package main

import (
	"net/http"
)

// Services
func LoadAccount(w http.ResponseWriter, r *http.Request) {
	/*
		fmt.Printf("[%s] - Request from %s \n", time.Now().Format(time.RFC3339), r.RemoteAddr)
		// Check unauthorized. Replace this Authorization token by a valid one
		// by automatic generation and / or a new and dedicated web service
		if r.UserAgent() != "Workstation Probing Agent" || r.Header.Get("Token") != "Jkd855c6x9Aqcf" {
			w.Header().Set("Content-Type", "application/json; charset=UTF-8")
			w.WriteHeader(http.StatusForbidden)
			panic("Non authorized access detected")
		}

		vars := mux.Vars(r)
		AccountId := vars["AccountId"]

		qChannel := make(chan AccountModel)
		var wg sync.WaitGroup
		wg.Add(1)
		//go grReadDir(concretePath, "", qChannel, &wg)

		w.Header().Set("Content-Type", "application/json; charset=UTF-8")
		w.WriteHeader(http.StatusOK)

		if err := json.NewEncoder(w).Encode(<-qChannel); err != nil {
			panic(err)
		}
	*/
}

func ManageAccount(w http.ResponseWriter, r *http.Request) {

}
