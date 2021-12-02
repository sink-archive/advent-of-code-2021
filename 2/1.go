package main

import (
	"fmt"
	"io/ioutil"
	"os"
	"strconv"
	"strings"
)

type instr struct {
	dir string
	num int
}

func main() {
	content, _ := ioutil.ReadFile(os.Args[1])

	raw := strings.Split(string(content), "\n")
	parsed := []instr{}
	for _, s := range raw {
		split := strings.Split(s, " ")
		i, _ := strconv.Atoi(split[1])
		parsed = append(parsed, instr{split[0], i})
	}

	depth, horiz := 0, 0
	for _, e := range parsed {
		if e.dir == "forward" {
			horiz += e.num
		} else if e.dir == "up" {
			depth -= e.num
		} else if e.dir == "down" {
			depth += e.num
		}
	}

	fmt.Println(depth * horiz)
}
