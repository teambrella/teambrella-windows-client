        static void getMoney(string [] addresses)
        {
            var chain = new Chain(Network.Main, new StreamObjectStream<ChainChange>(File.Open("MainChain.dat", FileMode.OpenOrCreate)));
            var node = Node.ConnectToLocal(Network.Main);
            node.VersionHandshake();
            node.SynchronizeChain(chain);

            BlockStore store = new BlockStore("D:\\BitcoinBlockchain\\blocks", Network.Main); //the folder is the blocks folder of all blocks saved by Bitcoin-QT, the original bitcoin client application, this "store" can only browse blocks sequencially
            IndexedBlockStore index = new IndexedBlockStore(new SQLiteNoSqlRepository("PlayIndex"), store); //Save a SqlLite index in a file called PlayIndex
            index.ReIndex(); //Index, to do one time only to fill the index

            var headers =
                chain.ToEnumerable(false)
                .SkipWhile(c => c.Header.BlockTime < new DateTime(2015, 2, 1))
                .TakeWhile(c => c.Header.BlockTime < DateTime.Now)
                .ToArray();

            var target = Money.Parse("1100");
            var margin = Money.Parse("1");
            var en = new CultureInfo("en-US");

            FileStream fs = File.Open("logs", FileMode.Create);
            var writer = new StreamWriter(fs);
            writer.WriteLine("time,height,txid,value");

            var lines = from header in headers
                        let block = index.Get(header.HashBlock)
                        from tx in block.Transactions
                        from txout in tx.Outputs
                        //where header.Height == 344681 
                        //where addresses.Contains(txout.ScriptPubKey.GetDestinationAddress(Network.Main).ToString())
                        //where target - margin < txout.Value && txout.Value < target + margin
                        select new
                        {
                            Block = block,
                            Height = header.Height,
                            Transaction = tx,
                            TxOut = txout
                        };
            foreach (var line in lines)
            {
                if (line.Height == 344681)
                writer.WriteLine(
                    line.Block.Header.BlockTime.ToString(en) + "," +
                    line.Height + "," +
                    line.Transaction.GetHash() + "," +
                    line.TxOut.ScriptPubKey.GetDestinationAddress(Network.Main) + "," +
                    line.TxOut.Value.ToString());
            }
            writer.Flush();
        }
