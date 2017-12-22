package main

import (
	"crypto/md5"
	"encoding/json"
	"fmt"
	"io"
	"log"
	"os"
	"strconv"
	"sync"
	"testing"
	"time"
)

func TestReadDir(t *testing.T) {
	tests := []struct {
		DirPath string
	}{
		{".\\Addons\\"},
	}

	for _, tt := range tests {
		start := time.Now()

		jsonMarshalled, _ := json.Marshal(ReadDir(tt.DirPath, ""))
		elapsed := time.Since(start)
		fmt.Println(string(jsonMarshalled))
		log.Printf("Function took %s", elapsed)
	}
}

func TestGrDiscover(t *testing.T) {
	tests := []struct {
		DirPath string
	}{
		{".\\Addons\\"},
	}
	qChannel := make(chan AddonModel, 1000)
	var wg sync.WaitGroup

	for _, tt := range tests {
		start := time.Now()
		wg.Add(1)
		go grDiscover(tt.DirPath, "", qChannel, &wg)

		log.Println("Waiting all channels")
		wg.Wait()

		log.Println("Done waiting the goroutines in main thread")

		for i := 0; i < 10; i++ {
			if len(qChannel) != 0 {
				if i > 0 {
					i--
				}

				jsonMarshalled, _ := json.Marshal(<-qChannel)
				log.Println(string(jsonMarshalled))

			}
		}

		close(qChannel)
		elapsed := time.Since(start)

		log.Printf("Function took %s", elapsed)
	}
}

func TestDownload(t *testing.T) {
	tests := []struct {
		DirPath string
	}{
		{"P2PHosting"},
	}

	for _, tt := range tests {
		start := time.Now()

		log.Println("Waiting all channels")
		Module := tt.DirPath

		log.Println("Done waiting the goroutines in main thread")
		h := md5.New()
		io.WriteString(h, Module)

		log.Println("Module found: " + Module)
		out, err := os.Open(".\\Addons\\" + fmt.Sprintf("%x", h.Sum(nil)) + "\\" + Module + ".dll")

		if err != nil {
			panic(err)
		}

		defer out.Close()

		FileHeader := make([]byte, 512)
		out.Read(FileHeader)

		//Get the file size
		FileStat, _ := out.Stat()                          //Get info from file
		FileSize := strconv.FormatInt(FileStat.Size(), 10) //Get file size as a string

		log.Println("Stats " + FileSize)
		//Send the file
		//We read 512 bytes from the file already so we reset the offset back to 0
		out.Seek(0, 0)

		elapsed := time.Since(start)

		log.Printf("Function took %s", elapsed)
	}
}
