package main

import (
	"fmt"
	"log"
	"testing"
	"time"
)

func Test_readAccount(t *testing.T) {
	tests := []struct {
		DirPath string
	}{
		{"..\\WorkstationBrowser\\UserContent\\FileTracker\\Workstation\\Comments\\"},
	}
	for _, tt := range tests {
		start := time.Now()

		elapsed := time.Since(start)
		fmt.Println(string(jsonMarshalled))
		log.Printf("Function took %s", elapsed)
	}
}
