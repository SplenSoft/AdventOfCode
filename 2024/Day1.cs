using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

internal class Day1 : Day
{
    #region Properties

    public override int Year => 2024;

    public override int DayNumber => 1;

    public override string Synopsis => @"--- Day 1: Historian Hysteria ---
The Chief Historian is always present for the big Christmas sleigh launch, but nobody has seen him in months! Last anyone heard, he was visiting locations that are historically significant to the North Pole; a group of Senior Historians has asked you to accompany them as they check the places they think he was most likely to visit.

As each location is checked, they will mark it on their list with a star. They figure the Chief Historian must be in one of the first fifty places they'll look, so in order to save Christmas, you need to help them get fifty stars on their list before Santa takes off on December 25th.

Collect stars by solving puzzles. Two puzzles will be made available on each day in the Advent calendar; the second puzzle is unlocked when you complete the first. Each puzzle grants one star. Good luck!

You haven't even left yet and the group of Elvish Senior Historians has already hit a problem: their list of locations to check is currently empty. Eventually, someone decides that the best place to check first would be the Chief Historian's office.

Upon pouring into the office, everyone confirms that the Chief Historian is indeed nowhere to be found. Instead, the Elves discover an assortment of notes and lists of historically significant locations! This seems to be the planning the Chief Historian was doing before he left. Perhaps these notes can be used to determine which locations to search?

Throughout the Chief's office, the historically significant locations are listed not by name but by a unique number called the location ID. To make sure they don't miss anything, The Historians split into two groups, each searching the office and trying to create their own complete list of location IDs.

There's just one problem: by holding the two lists up side by side (your puzzle input), it quickly becomes clear that the lists aren't very similar. Maybe you can help The Historians reconcile their lists?

For example:

3   4
4   3
2   5
1   3
3   9
3   3
Maybe the lists are only off by a small amount! To find out, pair up the numbers and measure how far apart they are. Pair up the smallest number in the left list with the smallest number in the right list, then the second-smallest left number with the second-smallest right number, and so on.

Within each pair, figure out how far apart the two numbers are; you'll need to add up all of those distances. For example, if you pair up a 3 from the left list with a 7 from the right list, the distance apart is 4; if you pair up a 9 with a 3, the distance apart is 6.

In the example list above, the pairs and distances would be as follows:

The smallest number in the left list is 1, and the smallest number in the right list is 3. The distance between them is 2.
The second-smallest number in the left list is 2, and the second-smallest number in the right list is another 3. The distance between them is 1.
The third-smallest number in both lists is 3, so the distance between them is 0.
The next numbers to pair up are 3 and 4, a distance of 1.
The fifth-smallest numbers in each list are 3 and 5, a distance of 2.
Finally, the largest number in the left list is 4, while the largest number in the right list is 9; these are a distance 5 apart.
To find the total distance between the left list and the right list, add up the distances between all of the pairs you found. In the example above, this is 2 + 1 + 0 + 1 + 2 + 5, a total distance of 11!

Your actual left and right lists contain many location IDs. What is the total distance between your lists?

--- Part Two ---
Your analysis only confirmed what everyone feared: the two lists of location IDs are indeed very different.

Or are they?

The Historians can't agree on which group made the mistakes or how to read most of the Chief's handwriting, but in the commotion you notice an interesting detail: a lot of location IDs appear in both lists! Maybe the other numbers aren't location IDs at all but rather misinterpreted handwriting.

This time, you'll need to figure out exactly how often each number from the left list appears in the right list. Calculate a total similarity score by adding up each number in the left list after multiplying it by the number of times that number appears in the right list.

Here are the same example lists again:

3   4
4   3
2   5
1   3
3   9
3   3
For these example lists, here is the process of finding the similarity score:

The first number in the left list is 3. It appears in the right list three times, so the similarity score increases by 3 * 3 = 9.
The second number in the left list is 4. It appears in the right list once, so the similarity score increases by 4 * 1 = 4.
The third number in the left list is 2. It does not appear in the right list, so the similarity score does not increase (2 * 0 = 0).
The fourth number, 1, also does not appear in the right list.
The fifth number, 3, appears in the right list three times; the similarity score increases by 9.
The last number, 3, appears in the right list three times; the similarity score again increases by 9.
So, for these example lists, the similarity score at the end of this process is 31 (9 + 4 + 0 + 0 + 9 + 9).

Once again consider your left and right lists. What is their similarity score?";

    public override string SampleInput => "77710   11556\r\n22632   23674\r\n82229   77288\r\n35788   30924\r\n84000   63702\r\n28350   62605\r\n15185   47495\r\n59530   63702\r\n38142   60772\r\n53694   41728\r\n38645   53443\r\n14632   43457\r\n54935   60772\r\n80251   67650\r\n45254   80940\r\n92045   48739\r\n88570   62608\r\n36464   14863\r\n15072   50428\r\n47732   12515\r\n66594   38710\r\n49622   63960\r\n38988   83798\r\n80235   95974\r\n66138   48396\r\n24678   63702\r\n89750   21245\r\n49637   60225\r\n66174   33783\r\n31929   60355\r\n86217   38710\r\n63702   64237\r\n10029   40993\r\n18724   11247\r\n86376   33183\r\n38846   77319\r\n71807   81452\r\n95125   65995\r\n14313   48739\r\n43968   82639\r\n95579   67299\r\n34700   57990\r\n95295   86925\r\n77804   39861\r\n23393   68293\r\n16675   33218\r\n86282   15372\r\n23383   44772\r\n93238   77732\r\n48968   55633\r\n13865   82878\r\n94041   30924\r\n58961   57990\r\n95054   99680\r\n38556   11247\r\n49445   49423\r\n12517   17577\r\n16458   62967\r\n70240   11247\r\n70209   13865\r\n44469   37707\r\n44143   76913\r\n40530   67650\r\n77107   57609\r\n52160   33783\r\n21898   53452\r\n60672   67910\r\n78399   38710\r\n65701   18077\r\n44093   31621\r\n10717   89642\r\n61143   33783\r\n46421   36377\r\n25600   86925\r\n71610   67650\r\n84942   88880\r\n18709   63702\r\n35967   62840\r\n59430   33783\r\n99613   11247\r\n21285   53061\r\n45961   63960\r\n64708   61045\r\n18663   38142\r\n24076   81982\r\n65173   33218\r\n99985   38090\r\n54288   50656\r\n45599   57990\r\n71134   86925\r\n21756   47991\r\n61585   96229\r\n30625   28042\r\n20187   96905\r\n53557   36740\r\n97635   85201\r\n74537   26837\r\n47344   89993\r\n23434   81337\r\n22578   52652\r\n64334   95857\r\n31984   23357\r\n19285   91889\r\n89380   60225\r\n68178   55027\r\n60534   92951\r\n92947   36269\r\n19390   21611\r\n36751   61641\r\n46265   86925\r\n64682   62605\r\n82939   21611\r\n62893   68731\r\n80218   33783\r\n82578   47062\r\n38606   47991\r\n64066   32805\r\n73995   96279\r\n59980   81452\r\n35206   49067\r\n63735   22549\r\n69054   43457\r\n39028   33503\r\n47551   63960\r\n23082   33555\r\n46069   99239\r\n25862   21611\r\n29188   80530\r\n80159   72409\r\n89316   15281\r\n54325   96229\r\n20285   94652\r\n70669   60225\r\n38999   72702\r\n85965   38803\r\n29799   32822\r\n59668   67650\r\n51559   66111\r\n13066   40993\r\n83004   30924\r\n10791   93513\r\n49496   38142\r\n73531   13865\r\n69050   96905\r\n69139   86925\r\n38483   67650\r\n30305   30924\r\n81076   76125\r\n15160   86181\r\n16189   56515\r\n22900   81452\r\n73271   82195\r\n45974   47969\r\n36115   10740\r\n59422   43457\r\n85666   59898\r\n82750   60225\r\n40947   99239\r\n75465   61804\r\n54504   94607\r\n62259   57990\r\n61506   76849\r\n93497   89296\r\n86487   26837\r\n41882   90643\r\n88793   12950\r\n39763   99728\r\n93555   26837\r\n12393   94358\r\n74543   62605\r\n88378   13865\r\n50221   21611\r\n27299   21611\r\n11114   69673\r\n65791   40954\r\n83752   25848\r\n18745   66574\r\n74882   15777\r\n48692   92817\r\n44362   33783\r\n61242   33713\r\n75950   97325\r\n57627   78921\r\n27136   37764\r\n39092   75180\r\n57990   47495\r\n61045   38333\r\n92827   42676\r\n24438   68802\r\n47626   96905\r\n37175   21756\r\n73009   41330\r\n96229   91479\r\n11873   81197\r\n25332   28939\r\n93039   26837\r\n44499   54251\r\n19265   33218\r\n31294   48739\r\n21611   85477\r\n47571   35343\r\n41758   81197\r\n89804   57990\r\n48279   92182\r\n94594   66451\r\n57016   89158\r\n17266   86925\r\n81813   58727\r\n13336   38200\r\n94562   21611\r\n38748   27800\r\n43566   38115\r\n33641   16072\r\n32705   13865\r\n91130   23928\r\n25131   25225\r\n36909   58299\r\n42651   60225\r\n15671   86925\r\n55940   79543\r\n54192   93612\r\n35033   46154\r\n45535   93612\r\n67381   33783\r\n19212   43554\r\n61797   57334\r\n31432   63702\r\n67624   63702\r\n68653   21925\r\n68954   19906\r\n59967   19319\r\n92360   62605\r\n62287   81197\r\n45573   67650\r\n36328   86925\r\n61960   54935\r\n52269   30924\r\n29735   38710\r\n17036   86925\r\n41095   43262\r\n78536   69197\r\n66416   38710\r\n16855   26837\r\n34731   88069\r\n87905   57921\r\n94906   38142\r\n51691   60772\r\n23074   40993\r\n21975   30538\r\n98269   63247\r\n18510   11247\r\n75157   81452\r\n81502   96229\r\n42393   40392\r\n33218   59424\r\n27267   81197\r\n79787   64276\r\n93536   44879\r\n48011   86126\r\n31968   37764\r\n29136   93822\r\n31415   38142\r\n82576   57290\r\n52268   76837\r\n92810   86925\r\n72140   30880\r\n53488   77275\r\n92207   29581\r\n87280   30659\r\n37835   33783\r\n13844   33707\r\n36050   15281\r\n99005   47991\r\n28786   38142\r\n47468   63702\r\n25603   42097\r\n26727   78314\r\n49976   20189\r\n62921   45423\r\n52044   21611\r\n17790   89889\r\n88780   18585\r\n34331   38142\r\n67726   74641\r\n57916   47495\r\n63452   79115\r\n78967   39701\r\n94997   37764\r\n42030   96311\r\n60590   30659\r\n78384   86925\r\n92241   99239\r\n51851   98205\r\n35112   11955\r\n94174   93612\r\n56105   34772\r\n82931   83183\r\n85755   21756\r\n41762   11247\r\n29979   30538\r\n64165   50082\r\n70021   82923\r\n59604   50033\r\n13201   97846\r\n50277   79961\r\n50619   21580\r\n64368   62605\r\n83879   71346\r\n27839   21580\r\n26011   16779\r\n69345   93082\r\n41187   45082\r\n24601   57403\r\n89017   39275\r\n13283   63960\r\n31256   13865\r\n78623   40494\r\n39962   60772\r\n77663   75759\r\n26038   96905\r\n73316   33938\r\n92999   63310\r\n70166   81452\r\n76098   13228\r\n14891   73495\r\n28141   90952\r\n39299   28133\r\n15281   73967\r\n64842   69333\r\n95559   89761\r\n17538   13099\r\n76799   89172\r\n22138   21687\r\n49158   40993\r\n33693   62605\r\n55981   94587\r\n83142   79932\r\n21077   85487\r\n23175   30659\r\n80364   52407\r\n42472   57021\r\n29236   63702\r\n14255   81197\r\n38418   38658\r\n51859   99239\r\n45882   24944\r\n61324   62605\r\n61841   14654\r\n80388   49151\r\n29747   37942\r\n72476   85550\r\n16252   35104\r\n29781   40993\r\n44328   57990\r\n54660   26837\r\n59197   60225\r\n39215   26403\r\n29033   87810\r\n74751   13865\r\n11363   81452\r\n45067   13865\r\n69924   75263\r\n58362   14620\r\n99338   97975\r\n75898   43457\r\n96905   16625\r\n70983   76517\r\n14236   74084\r\n96360   24647\r\n86881   50855\r\n26274   21580\r\n40600   30924\r\n58564   40993\r\n99226   35194\r\n87750   47107\r\n97441   60772\r\n35567   84318\r\n21972   77712\r\n78537   60772\r\n67606   33783\r\n87472   54880\r\n40590   39337\r\n65012   40993\r\n34228   86925\r\n67523   60772\r\n59292   92825\r\n58678   51248\r\n84088   74861\r\n15637   67061\r\n84670   34083\r\n65027   35840\r\n36693   78427\r\n13907   63494\r\n21482   63960\r\n93441   20115\r\n52171   52940\r\n19489   81197\r\n15078   62605\r\n83077   30659\r\n63960   60225\r\n46050   80683\r\n38700   96905\r\n63198   38142\r\n41302   48739\r\n68920   57324\r\n45369   27878\r\n80613   35343\r\n42072   18174\r\n72266   67650\r\n33109   59561\r\n27044   94017\r\n58397   99239\r\n99287   26837\r\n75188   26837\r\n35603   96905\r\n72118   64189\r\n52217   89498\r\n22261   15916\r\n47554   64893\r\n37504   93612\r\n37585   81197\r\n57160   99788\r\n58979   86925\r\n86068   17478\r\n30924   67650\r\n91757   33783\r\n41612   62605\r\n49882   58770\r\n58123   91694\r\n19042   25712\r\n74025   54935\r\n72215   80755\r\n59251   94914\r\n51699   82641\r\n38983   81452\r\n52780   27519\r\n19446   23721\r\n44414   93461\r\n10848   63498\r\n13269   77958\r\n55135   86925\r\n94862   96905\r\n61608   71892\r\n32466   63702\r\n73641   77222\r\n88895   60772\r\n30659   43691\r\n79642   26837\r\n14338   62605\r\n64448   98623\r\n56553   26837\r\n40861   95000\r\n27892   38142\r\n66064   56112\r\n27121   67650\r\n47531   38710\r\n25848   38142\r\n14271   82868\r\n10228   60225\r\n87793   84307\r\n47175   11247\r\n17169   21756\r\n27721   32097\r\n92418   56108\r\n66355   95179\r\n44735   14992\r\n56507   47495\r\n28974   30659\r\n25283   47495\r\n49106   30659\r\n61397   20474\r\n57660   37764\r\n95424   82164\r\n10121   68532\r\n49037   21756\r\n16018   54935\r\n54171   21580\r\n80053   62605\r\n54412   86925\r\n43192   98063\r\n53374   30538\r\n65521   28042\r\n95061   23181\r\n94014   96905\r\n24774   45834\r\n28399   48739\r\n97698   26837\r\n46147   65487\r\n84884   13865\r\n52030   67650\r\n93282   25692\r\n79313   96229\r\n58667   57990\r\n47096   12206\r\n87391   13255\r\n99072   77986\r\n50703   38710\r\n87988   66679\r\n71641   20252\r\n59311   96905\r\n71220   26837\r\n67650   37764\r\n59553   50172\r\n31948   78353\r\n34761   62033\r\n87610   12483\r\n31841   10666\r\n94372   64616\r\n60219   93612\r\n22359   20993\r\n97102   38142\r\n77624   33783\r\n24951   60772\r\n37488   15301\r\n11723   48739\r\n39666   30924\r\n32108   63702\r\n83851   34362\r\n96210   93612\r\n86925   96905\r\n45896   30538\r\n65334   17294\r\n72470   70423\r\n30711   64283\r\n41405   14988\r\n43167   67650\r\n96945   74134\r\n86918   21648\r\n73197   99239\r\n12883   30924\r\n68421   66131\r\n52765   38142\r\n90834   21611\r\n45258   64964\r\n47579   75382\r\n32277   35346\r\n71673   46895\r\n37595   87784\r\n48519   41837\r\n29412   96905\r\n96414   21611\r\n61644   21815\r\n81808   62605\r\n30538   81530\r\n34537   63702\r\n93380   28460\r\n87906   60225\r\n10146   40993\r\n47991   40365\r\n67034   48739\r\n83743   12925\r\n10147   37764\r\n26378   87307\r\n40198   47495\r\n67130   54935\r\n83983   93612\r\n72931   59552\r\n95192   32462\r\n95378   15281\r\n96684   30924\r\n95810   94119\r\n84753   83149\r\n40769   68570\r\n17455   63702\r\n81452   30538\r\n10731   30614\r\n45236   60225\r\n86568   60225\r\n23497   40337\r\n51480   19838\r\n13760   33886\r\n45686   60296\r\n96295   75032\r\n76955   89344\r\n45913   68223\r\n87186   11247\r\n31427   71637\r\n55276   85526\r\n38894   67650\r\n21981   42123\r\n22349   96229\r\n57614   21611\r\n94900   30538\r\n66311   93612\r\n55885   89479\r\n50423   37764\r\n26215   21076\r\n48434   60772\r\n40955   26850\r\n39151   66713\r\n70875   94570\r\n27665   95982\r\n18538   40993\r\n13492   67650\r\n70499   47991\r\n77683   54750\r\n67747   30924\r\n71323   72807\r\n59706   24034\r\n38778   40993\r\n87807   25447\r\n70172   84749\r\n26837   38142\r\n23133   37764\r\n64159   37764\r\n52185   69425\r\n76687   41712\r\n63845   30659\r\n35561   13247\r\n43934   63702\r\n40181   85421\r\n32289   96905\r\n65398   67650\r\n71139   54935\r\n73790   47495\r\n45466   76229\r\n58332   67014\r\n61971   72358\r\n69121   63726\r\n39542   93612\r\n22484   68967\r\n55587   48739\r\n60432   18975\r\n94726   64078\r\n24284   14730\r\n14909   99239\r\n90684   11247\r\n32649   33783\r\n65137   47495\r\n77217   91427\r\n21116   47495\r\n83895   96905\r\n96601   27302\r\n55754   40024\r\n79944   85355\r\n16002   66193\r\n90129   11247\r\n30213   70372\r\n78641   83755\r\n63174   13530\r\n14543   10536\r\n90871   21580\r\n85852   79909\r\n34592   96905\r\n10211   33104\r\n97542   11247\r\n54594   33218\r\n63461   64351\r\n28007   25447\r\n99493   80756\r\n37563   33783\r\n81527   47495\r\n84654   81452\r\n18992   19077\r\n24263   25447\r\n69870   80090\r\n72716   33783\r\n97138   36996\r\n52068   63702\r\n29147   96229\r\n37530   64645\r\n33783   46150\r\n74846   60772\r\n16825   15281\r\n35345   62605\r\n69248   63960\r\n62605   40993\r\n48393   72178\r\n18517   63702\r\n83254   33783\r\n79495   30659\r\n66124   23376\r\n78999   80281\r\n96072   93612\r\n23506   12287\r\n28042   71962\r\n37764   29122\r\n27239   77167\r\n67379   96052\r\n50343   62639\r\n65800   37119\r\n73545   10601\r\n26147   48018\r\n11703   11247\r\n12981   63702\r\n10852   45862\r\n28206   37518\r\n98061   28757\r\n83811   58066\r\n23862   67650\r\n76612   22466\r\n10665   73928\r\n30081   85075\r\n63962   96905\r\n76010   84994\r\n27403   67650\r\n90077   63960\r\n74068   93612\r\n59069   67650\r\n48585   76889\r\n26308   62536\r\n94927   25428\r\n16522   88617\r\n42895   67650\r\n23882   25848\r\n88312   53998\r\n93856   55795\r\n37116   62605\r\n78120   42683\r\n79015   12949\r\n60074   54593\r\n41372   30538\r\n92369   25447\r\n46454   10552\r\n90499   13865\r\n95683   47495\r\n26207   15402\r\n41663   38142\r\n45778   96087\r\n82536   26837\r\n73671   62605\r\n90830   70767\r\n98755   11247\r\n40667   21580\r\n68269   68306\r\n27166   38710\r\n52502   36604\r\n90650   39860\r\n60073   67650\r\n82795   80557\r\n93494   67650\r\n61210   75404\r\n65316   65293\r\n31264   86574\r\n77373   98352\r\n30739   81452\r\n85346   62605\r\n66664   21611\r\n99662   37764\r\n29578   98074\r\n57246   60772\r\n29093   22422\r\n59393   33582\r\n69790   62469\r\n33276   30114\r\n80189   96666\r\n66119   26837\r\n19784   62605\r\n81184   41088\r\n35076   33783\r\n67704   61250\r\n48739   33218\r\n55762   13865\r\n89013   26837\r\n37137   21611\r\n88909   61336\r\n32196   30538\r\n86724   29523\r\n17981   54398\r\n56448   60256\r\n80823   54221\r\n83300   27328\r\n88026   38142\r\n58336   93612\r\n65810   77527\r\n24714   13865\r\n17929   86925\r\n11327   77128\r\n55034   58912\r\n44769   86797\r\n13322   55659\r\n64372   93494\r\n36477   21611\r\n23642   60772\r\n55724   60772\r\n26667   36225\r\n45210   96905\r\n11247   46836\r\n78918   90137\r\n13992   47495\r\n41326   60225\r\n47903   26837\r\n35926   77384\r\n27221   95442\r\n98695   63702\r\n93775   43732\r\n97744   78882\r\n81628   30659\r\n60225   63960\r\n36543   23250\r\n98904   30924\r\n79272   76007\r\n91916   45599\r\n21068   14578\r\n44525   49606\r\n26951   60225\r\n85562   81197\r\n79077   93612\r\n91418   74576\r\n64125   48739\r\n25949   65857\r\n36366   30045\r\n51836   21611\r\n59493   30924\r\n14600   31019\r\n51609   40993\r\n56673   93612\r\n85240   33218\r\n89574   15281\r\n30058   15281\r\n29464   46074\r\n94282   15086\r\n28521   53216\r\n85293   55577\r\n71182   37764\r\n14518   19328\r\n60810   64463\r\n44155   71652\r\n50352   30924\r\n81271   37520\r\n56005   73595\r\n93426   99239\r\n48196   49136\r\n80048   21611\r\n99240   42592\r\n20530   33783\r\n60772   53594\r\n43174   36790\r\n69363   14739\r\n77407   35483\r\n37644   60772\r\n86726   87246\r\n98832   40993\r\n26660   79340\r\n67933   26574\r\n74474   48739\r\n62414   49360\r\n20524   21580\r\n57665   50944\r\n49993   62605\r\n39330   74779\r\n95735   60225\r\n54490   60772\r\n42736   94550\r\n18812   33783\r\n83965   71738\r\n96638   78706\r\n75468   93612\r\n23757   37754\r\n69579   44683\r\n98297   75616\r\n49322   34521\r\n34698   29727\r\n99972   40993\r\n52254   74431\r\n85235   30538\r\n65740   30778\r\n52565   47991\r\n33052   78908\r\n60013   96229\r\n83067   99496\r\n85558   60225\r\n27244   96905\r\n72402   26837\r\n91813   99239\r\n65555   60225\r\n35968   25069\r\n53737   61662\r\n70451   46554\r\n78329   73260\r\n55609   74063\r\n12781   60772\r\n66272   49941\r\n16118   86925\r\n26033   49617\r\n48390   63702\r\n30003   23436\r\n41542   91300\r\n76570   93612\r\n67639   63960\r\n63930   11333\r\n22066   36920\r\n83376   74684\r\n16104   21611\r\n35185   56286\r\n95216   44366\r\n22605   62605\r\n80979   30538\r\n14635   94665\r\n83619   30924\r\n92781   98532\r\n78861   38764\r\n62770   19341\r\n39746   25447\r\n93612   38710\r\n44092   62605\r\n77918   96905\r\n80257   30580\r\n17590   99272\r\n68229   76224\r\n54001   85765\r\n97469   99239\r\n37205   30538\r\n69048   57990\r\n14872   60739\r\n48571   99239\r\n23974   91218\r\n38710   14364\r\n31301   26090\r\n68179   99207\r\n67300   99239\r\n33247   34833\r\n21580   99865\r\n74274   97554\r\n20411   47495\r\n40542   30538\r\n67917   89131\r\n99902   40838\r\n40993   21580\r\n97436   35819\r\n23800   60566\r\n86595   88748\r\n78666   19083\r\n47495   41135\r\n98215   38710\r\n22482   80507\r\n77086   67050\r\n53308   14323\r\n75294   88625\r\n82489   29928\r\n49093   32934\r\n96378   68073\r\n37250   71257\r\n16009   21611\r\n64164   47014\r\n69019   47495\r\n63457   43171\r\n50592   27922\r\n46024   70597\r\n49667   96082\r\n40856   11247\r\n42134   99956\r\n26021   55068\r\n20318   98162\r\n71497   13865\r\n43457   38142\r\n91041   26837\r\n16291   60225\r\n25447   98873\r\n79232   30924\r\n61697   96016\r\n46796   41899\r\n12977   58039\r\n62416   65959\r\n13416   27187\r\n61371   93612\r\n89884   50535\r\n62007   79719\r\n97904   65662\r\n69865   57990\r\n10288   21580\r\n71558   28042\r\n55800   30659\r\n23853   10422\r\n20471   13865\r\n20564   18219\r\n44027   13804\r\n46965   87046\r\n76735   64178\r\n95328   85608\r\n43478   40522\r\n26456   96905\r\n53931   30924\r\n81197   67359\r\n18024   63702\r\n34943   26837\r\n15279   44819\r\n91142   63960\r\n46127   30538\r\n31830   21574\r\n17905   63960\r\n35343   48107\r\n75727   87321\r\n77205   25447\r\n62962   11247\r\n79177   55811\r\n33599   83446\r\n18797   80213\r\n84721   86925\r\n50446   21611\r\n68889   26952\r\n91051   17166\r\n35127   29638\r\n99239   43522\r\n99122   27157\r\n29947   39341\r\n21459   97910\r\n22063   15281\r\n63297   43457\r\n42167   11247";

    #endregion

    /// <param name="input">
    /// A string consisting of multiple lines of two side-by-side 
    /// numbers, separated by three spaces
    /// </param>
    public override string Solve(string input)
    {
        var regex = new Regex(@"(\d{5})\s{3}(\d{5})(?:\n|\r\n|$)");
        List<List<int>> lists = [[], []];

        foreach (Match match in regex.Matches(input))
            for (int i = 0; i < lists.Count; i++)
                lists[i].Add(int.Parse(match.Groups[i + 1].Value));

        for (int i = 0; i < lists.Count; i++)
            lists[i] = [.. lists[i].Order()];

        int part1 = 0;
        int part2 = 0;

        for (int i = 0; i < lists[0].Count; i++)
        {
            int item1 = lists[0][i];
            part1 += Math.Abs(item1 - lists[1][i]);
            int appearances = 0;

            for (int j = 0; j < lists[1].Count; j++)
                if (item1 == lists[1][j])
                    appearances++;

            part2 += item1 * appearances;
        }
            
        return $"Part 1 solution: {part1}\nPart 2 solution: {part2}";
    }
}