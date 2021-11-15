def main():
    f = open("testInput")
    
    fileContent = f.read()
    
    list = [int(s) for s in fileContent.split(',')]
    
    sum = 0
    for n in list:
        sum += n
    
    print(sum)
    
    f.close()

if __name__ == "__main__":
    main()