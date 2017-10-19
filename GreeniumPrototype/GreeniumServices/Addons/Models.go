package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"sync"
)

/*
	--------------- MODELS
*/

type AddonModel struct {
	ID   string `json:"ID"`
	Name string `json:"Name"`
	Path string `json:"Path"`
	Size int    `json:"Size"`
}

/*
	-------------------
*/

// Goroutines related

func ReadDir(DirPath string, parent string) []AddonModel {
	files, err := ioutil.ReadDir(DirPath)
	addons := make([]AddonModel, 0)

	if err != nil {
		fmt.Println("Error opening file:", err)
		return addons
	}

	for _, f := range files {
		if f.IsDir() == false {
			currentAddon := AddonModel{
				ID:   "",
				Name: f.Name(),
				Path: (DirPath + "\\" + f.Name()),
				Size: 0,
			}

			addons = append(addons, currentAddon)
		} else {
			addons = append(addons, ReadDir(DirPath+"\\"+f.Name(), parent+"/"+f.Name())...)
		}
	}

	return addons
}

// Goroutine discovery
func grDiscover(DirPath string, parent string, result chan AddonModel, wg *sync.WaitGroup) {
	defer wg.Done() // Done at the end, ofc

	log.Printf("Goroutine %s started", DirPath)

	files, err := ioutil.ReadDir(DirPath)
	if err != nil {
		log.Println("Error opening file:", err)
		result <- (AddonModel{})

		return // Do not continue
	}

	// Preparing space for routines
	totalLocks := 0
	qChannel := make(chan AddonModel, len(files)+20) // Margin
	var thisWaitingGroup sync.WaitGroup

	for _, f := range files {
		log.Printf("Element %s found", f.Name())
		if f.IsDir() == false {
			currentAddon := AddonModel{
				ID:   "",
				Name: f.Name(),
				Path: (DirPath + "\\" + f.Name()),
				Size: 0,
			}

			result <- currentAddon
		} else {
			thisWaitingGroup.Add(1)
			totalLocks++
			go grDiscover(DirPath+"\\"+f.Name(), parent+"/"+f.Name(), qChannel, &thisWaitingGroup)
		}
	}

	log.Printf("Waiting nested goroutines of %s", DirPath)
	thisWaitingGroup.Wait()

	log.Printf("Done waiting in %s", DirPath)

	close(qChannel)
	log.Printf("Routine Locks: %d", totalLocks)
	if len(qChannel) > 0 {
		log.Printf("Total responses: %d", len(qChannel))

		for response := range qChannel {

			log.Println("Response acquired: ", response)
			result <- response
		}

		log.Println("Channel draining done")

	}

	log.Printf("Goroutine %s terminated", DirPath)
	//wg.Done()
	//result <- query
}
