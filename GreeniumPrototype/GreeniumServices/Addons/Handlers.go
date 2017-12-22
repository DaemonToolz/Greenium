package main

import (
	"crypto/md5"
	"fmt"
	"io"
	"log"
	"net/http"
	"os"
	"strconv"
	"time"

	"github.com/gorilla/mux"
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

	vars := mux.Vars(r)
	Module := vars["module"]

	log.Printf("Downloading module %s ", Module)
	h := md5.New()
	io.WriteString(h, Module)

	out, err := os.Create(".\\Addons\\" + fmt.Sprintf("%x", h.Sum(nil)) + "\\" + Module + ".dll")
	if err != nil {
		panic(err)
	}

	defer out.Close()

	w.WriteHeader(http.StatusOK)

	FileHeader := make([]byte, 512)
	out.Read(FileHeader)
	FileContentType := http.DetectContentType(FileHeader)

	//Get the file size
	FileStat, _ := out.Stat()                          //Get info from file
	FileSize := strconv.FormatInt(FileStat.Size(), 10) //Get file size as a string

	//Send the headers
	w.Header().Set("Content-Disposition", "attachment; filename="+Module+".dll")
	w.Header().Set("Content-Type", FileContentType)
	w.Header().Set("Content-Length", FileSize)

	//Send the file
	//We read 512 bytes from the file already so we reset the offset back to 0
	out.Seek(0, 0)
	io.Copy(w, out) //'Copy' the file to the client
	//io.Copy(out, resp.Body)
}
