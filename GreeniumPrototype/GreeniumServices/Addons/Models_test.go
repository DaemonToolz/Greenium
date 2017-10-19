package main

import (
	"encoding/json"
	"fmt"
	"log"
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
