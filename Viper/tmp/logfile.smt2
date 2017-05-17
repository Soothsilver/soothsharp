(get-info :version)
; (:version "4.4.1")
; Input file is C:\Users\Soothsilver\AppData\Local\Temp\tmp2078.tmp
; Started: 2017-05-17 20:56:58
; Silicon.buildVersion: 1.1-SNAPSHOT 0e750e485a3f default 2017/01/04 14:11:46
; ------------------------------------------------------------
; Preamble start
; 
; ; /z3config.smt2
(set-option :print-success true) ; Boogie: false
(set-option :global-decls true) ; Boogie: default
(set-option :auto_config false) ; Usually a good idea
(set-option :smt.mbqi false)
(set-option :model.v2 true)
(set-option :smt.phase_selection 0)
(set-option :smt.restart_strategy 0)
(set-option :smt.restart_factor |1.5|)
(set-option :smt.arith.random_initial_value true)
(set-option :smt.case_split 3)
(set-option :smt.delay_units true)
(set-option :smt.delay_units_threshold 16)
(set-option :nnf.sk_hack true)
(set-option :smt.qi.eager_threshold 100)
(set-option :smt.qi.cost "(+ weight generation)")
(set-option :type_check true)
(set-option :smt.bv.reflect true)
; 
; ; /preamble.smt2
(declare-datatypes () ((
    $Snap ($Snap.unit)
    ($Snap.combine ($Snap.first $Snap) ($Snap.second $Snap)))))
(declare-sort $Ref 0)
(declare-const $Ref.null $Ref)
(define-sort $Perm () Real)
(define-const $Perm.Write $Perm 1.0)
(define-const $Perm.No $Perm 0.0)
(define-fun $Perm.isValidVar ((p $Perm)) Bool
	(<= $Perm.No p))
(define-fun $Perm.isReadVar ((p $Perm) (ub $Perm)) Bool
    (and ($Perm.isValidVar p)
         (not (= p $Perm.No))
         (< p $Perm.Write)))
(define-fun $Perm.min ((p1 $Perm) (p2 $Perm)) Real
    (ite (<= p1 p2) p1 p2))
(define-fun $Math.min ((a Int) (b Int)) Int
    (ite (<= a b) a b))
(define-fun $Math.clip ((a Int)) Int
    (ite (< a 0) 0 a))
(declare-sort $Seq<Int>)
(declare-sort $FVF<$Ref>)
(declare-sort $Set<$Snap>)
(declare-sort $PSF<$Snap>)
; /dafny_axioms/sequences_declarations_dafny.smt2 [Int]
(declare-fun $Seq.length ($Seq<Int>) Int)
(declare-fun $Seq.empty<Int> () $Seq<Int>)
(declare-fun $Seq.singleton (Int) $Seq<Int>)
(declare-fun $Seq.build ($Seq<Int> Int) $Seq<Int>)
(declare-fun $Seq.index ($Seq<Int> Int) Int)
(declare-fun $Seq.append ($Seq<Int> $Seq<Int>) $Seq<Int>)
(declare-fun $Seq.update ($Seq<Int> Int Int) $Seq<Int>)
(declare-fun $Seq.contains ($Seq<Int> Int) Bool)
(declare-fun $Seq.take ($Seq<Int> Int) $Seq<Int>)
(declare-fun $Seq.drop ($Seq<Int> Int) $Seq<Int>)
(declare-fun $Seq.equal ($Seq<Int> $Seq<Int>) Bool)
(declare-fun $Seq.sameuntil ($Seq<Int> $Seq<Int> Int) Bool)
; /dafny_axioms/sequences_int_declarations_dafny.smt2
(declare-fun $Seq.range (Int Int) $Seq<Int>)
; /dafny_axioms/sets_declarations_dafny.smt2 [Snap
(declare-fun $Set.in ($Snap $Set<$Snap>) Bool)
(declare-fun $Set.card ($Set<$Snap>) Int)
(declare-fun $Set.empty<$Snap> () $Set<$Snap>)
(declare-fun $Set.singleton ($Snap) $Set<$Snap>)
(declare-fun $Set.unionone ($Set<$Snap> $Snap) $Set<$Snap>)
(declare-fun $Set.union ($Set<$Snap> $Set<$Snap>) $Set<$Snap>)
(declare-fun $Set.disjoint ($Set<$Snap> $Set<$Snap>) Bool)
(declare-fun $Set.difference ($Set<$Snap> $Set<$Snap>) $Set<$Snap>)
(declare-fun $Set.intersection ($Set<$Snap> $Set<$Snap>) $Set<$Snap>)
(declare-fun $Set.subset ($Set<$Snap> $Set<$Snap>) Bool)
(declare-fun $Set.equal ($Set<$Snap> $Set<$Snap>) Bool)
; Declaring program functions
; Snapshot variable to be used during function verification
(declare-fun s@$ () $Snap)
; Declaring predicate trigger functions
; /dafny_axioms/sequences_axioms_dafny.smt2 [Int]
(assert (forall ((s $Seq<Int>) ) (! (<= 0 ($Seq.length s))
  :pattern ( ($Seq.length s))
  )))
(assert (= ($Seq.length $Seq.empty<Int>) 0))
(assert (forall ((s $Seq<Int>) ) (! (=> (= ($Seq.length s) 0) (= s $Seq.empty<Int>))
  :pattern ( ($Seq.length s))
  )))
(assert (forall ((t Int) ) (! (= ($Seq.length ($Seq.singleton t)) 1)
  :pattern ( ($Seq.length ($Seq.singleton t)))
  )))
(assert (forall ((s $Seq<Int>) (v Int) ) (! (= ($Seq.length ($Seq.build s v)) (+ 1 ($Seq.length s)))
  :pattern ( ($Seq.length ($Seq.build s v)))
  )))
(assert (forall ((s $Seq<Int>) (i Int) (v Int) ) (! (and
  (=> (= i ($Seq.length s)) (= ($Seq.index ($Seq.build s v) i) v))
  (=> (not (= i ($Seq.length s))) (= ($Seq.index ($Seq.build s v) i) ($Seq.index s i))))
  :pattern ( ($Seq.index ($Seq.build s v) i))
  )))
(assert (forall ((s0 $Seq<Int>) (s1 $Seq<Int>) ) (!
  (implies ; The implication was not in the Dafny version
    (and
      (not (= s0 $Seq.empty<Int>))
      (not (= s1 $Seq.empty<Int>)))
    (=
      ($Seq.length ($Seq.append s0 s1))
      (+ ($Seq.length s0) ($Seq.length s1))))
  :pattern ( ($Seq.length ($Seq.append s0 s1)))
  )))
(assert (forall ((t Int) ) (! (= ($Seq.index ($Seq.singleton t) 0) t)
  :pattern ( ($Seq.index ($Seq.singleton t) 0))
  )))
(assert (forall ((s $Seq<Int>)) (! ; The axiom was not in the Dafny version
  (= ($Seq.append s $Seq.empty<Int>) s)
  :pattern (($Seq.append s $Seq.empty<Int>))
  )))
(assert (forall ((s $Seq<Int>)) (! ; The axiom was not in the Dafny version
  (= ($Seq.append $Seq.empty<Int> s) s)
  :pattern (($Seq.append $Seq.empty<Int> s))
  )))
(assert (forall ((s0 $Seq<Int>) (s1 $Seq<Int>) (n Int) ) (!
  (implies ; The implication was not in the Dafny version
    (and
      (not (= s0 $Seq.empty<Int>))
      (not (= s1 $Seq.empty<Int>)))
    (and
      (=> (< n ($Seq.length s0)) (= ($Seq.index ($Seq.append s0 s1) n) ($Seq.index s0 n)))
      (=> (<= ($Seq.length s0) n) (= ($Seq.index ($Seq.append s0 s1) n) ($Seq.index s1 (- n ($Seq.length s0)))))))
  :pattern ( ($Seq.index ($Seq.append s0 s1) n))
  )))
(assert (forall ((s $Seq<Int>) (i Int) (v Int) ) (! (=> (and
  (<= 0 i)
  (< i ($Seq.length s))) (= ($Seq.length ($Seq.update s i v)) ($Seq.length s)))
  :pattern ( ($Seq.length ($Seq.update s i v)))
  )))
(assert (forall ((s $Seq<Int>) (i Int) (v Int) (n Int) ) (! (=> (and
  (<= 0 n)
  (< n ($Seq.length s))) (and
  (=> (= i n) (= ($Seq.index ($Seq.update s i v) n) v))
  (=> (not (= i n)) (= ($Seq.index ($Seq.update s i v) n) ($Seq.index s n)))))
  :pattern ( ($Seq.index ($Seq.update s i v) n))
  )))
(assert (forall ((s $Seq<Int>) (x Int) ) (!
  (and
    (=>
      ($Seq.contains s x)
      (exists ((i Int) ) (!
        (and
  (<= 0 i)
  (< i ($Seq.length s))
  (= ($Seq.index s i) x))
  :pattern ( ($Seq.index s i))
  )))
    (=>
      (exists ((i Int) ) (!
        (and
  (<= 0 i)
  (< i ($Seq.length s))
  (= ($Seq.index s i) x))
  :pattern ( ($Seq.index s i))
      ))
      ($Seq.contains s x)))
  :pattern ( ($Seq.contains s x))
  )))
(assert (forall ((x Int) ) (! (not ($Seq.contains $Seq.empty<Int> x))
  :pattern ( ($Seq.contains $Seq.empty<Int> x))
  )))
(assert (forall ((s0 $Seq<Int>) (s1 $Seq<Int>) (x Int) ) (! (and
  (=> ($Seq.contains ($Seq.append s0 s1) x) (or
  ($Seq.contains s0 x)
  ($Seq.contains s1 x)))
  (=> (or
  ($Seq.contains s0 x)
  ($Seq.contains s1 x)) ($Seq.contains ($Seq.append s0 s1) x)))
  :pattern ( ($Seq.contains ($Seq.append s0 s1) x))
  )))
(assert (forall ((s $Seq<Int>) (v Int) (x Int) ) (! (and
  (=> ($Seq.contains ($Seq.build s v) x) (or
  (= v x)
  ($Seq.contains s x)))
  (=> (or
  (= v x)
  ($Seq.contains s x)) ($Seq.contains ($Seq.build s v) x)))
  :pattern ( ($Seq.contains ($Seq.build s v) x))
  )))
(assert (forall ((s $Seq<Int>) (n Int) (x Int) ) (!
  (and
    (=>
      ($Seq.contains ($Seq.take s n) x)
      (exists ((i Int) ) (!
        (and
  (<= 0 i)
  (< i n)
  (< i ($Seq.length s))
  (= ($Seq.index s i) x))
  :pattern ( ($Seq.index s i))
  )))
    (=>
      (exists ((i Int) ) (!
        (and
  (<= 0 i)
  (< i n)
  (< i ($Seq.length s))
  (= ($Seq.index s i) x))
  :pattern ( ($Seq.index s i))
      ))
      ($Seq.contains ($Seq.take s n) x)))
  :pattern ( ($Seq.contains ($Seq.take s n) x))
  )))
(assert (forall ((s $Seq<Int>) (n Int) (x Int) ) (!
  (and
    (=>
      ($Seq.contains ($Seq.drop s n) x)
      (exists ((i Int) ) (!
        (and
  (<= 0 n)
  (<= n i)
  (< i ($Seq.length s))
  (= ($Seq.index s i) x))
  :pattern ( ($Seq.index s i))
  )))
    (=>
      (exists ((i Int) ) (!
        (and
  (<= 0 n)
  (<= n i)
  (< i ($Seq.length s))
  (= ($Seq.index s i) x))
  :pattern ( ($Seq.index s i))
      ))
      ($Seq.contains ($Seq.drop s n) x)))
  :pattern ( ($Seq.contains ($Seq.drop s n) x))
  )))
(assert (forall ((s0 $Seq<Int>) (s1 $Seq<Int>) ) (! (and
  (=> ($Seq.equal s0 s1) (and
  (= ($Seq.length s0) ($Seq.length s1))
  (forall ((j Int) ) (! (=> (and
  (<= 0 j)
  (< j ($Seq.length s0))) (= ($Seq.index s0 j) ($Seq.index s1 j)))
  :pattern ( ($Seq.index s0 j))
  :pattern ( ($Seq.index s1 j))
  ))))
  (=> (and
  (= ($Seq.length s0) ($Seq.length s1))
  (forall ((j Int) ) (! (=> (and
  (<= 0 j)
  (< j ($Seq.length s0))) (= ($Seq.index s0 j) ($Seq.index s1 j)))
  :pattern ( ($Seq.index s0 j))
  :pattern ( ($Seq.index s1 j))
  ))) ($Seq.equal s0 s1)))
  :pattern ( ($Seq.equal s0 s1))
  )))
(assert (forall ((a $Seq<Int>) (b $Seq<Int>) ) (! (=> ($Seq.equal a b) (= a b))
  :pattern ( ($Seq.equal a b))
  )))
(assert (forall ((s0 $Seq<Int>) (s1 $Seq<Int>) (n Int) ) (! (and
  (=> ($Seq.sameuntil s0 s1 n) (forall ((j Int) ) (! (=> (and
  (<= 0 j)
  (< j n)) (= ($Seq.index s0 j) ($Seq.index s1 j)))
  :pattern ( ($Seq.index s0 j))
  :pattern ( ($Seq.index s1 j))
  )))
  (=> (forall ((j Int) ) (! (=> (and
  (<= 0 j)
  (< j n)) (= ($Seq.index s0 j) ($Seq.index s1 j)))
  :pattern ( ($Seq.index s0 j))
  :pattern ( ($Seq.index s1 j))
  )) ($Seq.sameuntil s0 s1 n)))
  :pattern ( ($Seq.sameuntil s0 s1 n))
  )))
(assert (forall ((s $Seq<Int>) (n Int) ) (! (=> (<= 0 n) (and
  (=> (<= n ($Seq.length s)) (= ($Seq.length ($Seq.take s n)) n))
  (=> (< ($Seq.length s) n) (= ($Seq.length ($Seq.take s n)) ($Seq.length s)))))
  :pattern ( ($Seq.length ($Seq.take s n)))
  )))
(assert (forall ((s $Seq<Int>) (n Int) (j Int) ) (! (=> (and
  (<= 0 j)
  (< j n)
  (< j ($Seq.length s))) (= ($Seq.index ($Seq.take s n) j) ($Seq.index s j)))
  :pattern ( ($Seq.index ($Seq.take s n) j))
  :pattern (($Seq.index s j) ($Seq.take s n)) ; [XXX] Added 29-10-2015
  )))
(assert (forall ((s $Seq<Int>) (n Int) ) (! (=> (<= 0 n) (and
  (=> (<= n ($Seq.length s)) (= ($Seq.length ($Seq.drop s n)) (- ($Seq.length s) n)))
  (=> (< ($Seq.length s) n) (= ($Seq.length ($Seq.drop s n)) 0))))
  :pattern ( ($Seq.length ($Seq.drop s n)))
  )))
(assert (forall ((s $Seq<Int>) (n Int) (j Int) ) (! (=> (and
  (<= 0 n)
  (<= 0 j)
  (< j (- ($Seq.length s) n))) (= ($Seq.index ($Seq.drop s n) j) ($Seq.index s (+ j n))))
  :pattern ( ($Seq.index ($Seq.drop s n) j))
  )))
(assert (forall ((s $Seq<Int>) (n Int) (k Int) ) (! ; [XXX] Added 29-10-2015
  (=>
    (and
      (<= 0 n)
      (<= n k)
      (< k ($Seq.length s)))
    (=
      ($Seq.index ($Seq.drop s n) (- k n))
      ($Seq.index s k)))
  :pattern (($Seq.index s k) ($Seq.drop s n))
  )))
(assert (forall ((s $Seq<Int>) (i Int) (v Int) (n Int) ) (! (=> (and
  (<= 0 i)
  (< i n)
  (<= n ($Seq.length s))) (= ($Seq.take ($Seq.update s i v) n) ($Seq.update ($Seq.take s n) i v)))
  :pattern ( ($Seq.take ($Seq.update s i v) n))
  )))
(assert (forall ((s $Seq<Int>) (i Int) (v Int) (n Int) ) (! (=> (and
  (<= n i)
  (< i ($Seq.length s))) (= ($Seq.take ($Seq.update s i v) n) ($Seq.take s n)))
  :pattern ( ($Seq.take ($Seq.update s i v) n))
  )))
(assert (forall ((s $Seq<Int>) (i Int) (v Int) (n Int) ) (! (=> (and
  (<= 0 n)
  (<= n i)
  (< i ($Seq.length s))) (= ($Seq.drop ($Seq.update s i v) n) ($Seq.update ($Seq.drop s n) (- i n) v)))
  :pattern ( ($Seq.drop ($Seq.update s i v) n))
  )))
(assert (forall ((s $Seq<Int>) (i Int) (v Int) (n Int) ) (! (=> (and
  (<= 0 i)
  (< i n)
  (< n ($Seq.length s))) (= ($Seq.drop ($Seq.update s i v) n) ($Seq.drop s n)))
  :pattern ( ($Seq.drop ($Seq.update s i v) n))
  )))
(assert (forall ((s $Seq<Int>) (v Int) (n Int) ) (! (=> (and
  (<= 0 n)
  (<= n ($Seq.length s))) (= ($Seq.drop ($Seq.build s v) n) ($Seq.build ($Seq.drop s n) v)))
  :pattern ( ($Seq.drop ($Seq.build s v) n))
  )))
(assert (forall ((x Int) (y Int)) (!
  (iff
    ($Seq.contains ($Seq.singleton x) y)
    (= x y))
  :pattern (($Seq.contains ($Seq.singleton x) y))
  )))
; /dafny_axioms/sequences_int_axioms_dafny.smt2
(assert (forall ((min Int) (max Int) ) (! (and
  (=> (< min max) (= ($Seq.length ($Seq.range min max)) (- max min)))
  (=> (<= max min) (= ($Seq.length ($Seq.range min max)) 0)))
   :pattern ( ($Seq.length ($Seq.range min max)))
  )))
(assert (forall ((min Int) (max Int) (j Int) ) (! (=> (and
  (<= 0 j)
  (< j (- max min))) (= ($Seq.index ($Seq.range min max) j) (+ min j)))
   :pattern ( ($Seq.index ($Seq.range min max) j))
  )))
(assert (forall ((min Int) (max Int) (v Int) ) (! (and
  (=> ($Seq.contains ($Seq.range min max) v) (and
  (<= min v)
  (< v max)))
  (=> (and
  (<= min v)
  (< v max)) ($Seq.contains ($Seq.range min max) v)))
   :pattern ( ($Seq.contains ($Seq.range min max) v))
  )))
; Declaring additional sort wrappers
(declare-fun $SortWrappers.$PermTo$Snap ($Perm) $Snap)
(declare-fun $SortWrappers.$SnapTo$Perm ($Snap) $Perm)
(assert (forall ((x $Perm)) (!
    (= x ($SortWrappers.$SnapTo$Perm($SortWrappers.$PermTo$Snap x)))
    :pattern (($SortWrappers.$PermTo$Snap x))
    :qid |$Snap.$Perm|
    )))
(declare-fun $SortWrappers.$RefTo$Snap ($Ref) $Snap)
(declare-fun $SortWrappers.$SnapTo$Ref ($Snap) $Ref)
(assert (forall ((x $Ref)) (!
    (= x ($SortWrappers.$SnapTo$Ref($SortWrappers.$RefTo$Snap x)))
    :pattern (($SortWrappers.$RefTo$Snap x))
    :qid |$Snap.$Ref|
    )))
(declare-fun $SortWrappers.BoolTo$Snap (Bool) $Snap)
(declare-fun $SortWrappers.$SnapToBool ($Snap) Bool)
(assert (forall ((x Bool)) (!
    (= x ($SortWrappers.$SnapToBool($SortWrappers.BoolTo$Snap x)))
    :pattern (($SortWrappers.BoolTo$Snap x))
    :qid |$Snap.Bool|
    )))
(declare-fun $SortWrappers.IntTo$Snap (Int) $Snap)
(declare-fun $SortWrappers.$SnapToInt ($Snap) Int)
(assert (forall ((x Int)) (!
    (= x ($SortWrappers.$SnapToInt($SortWrappers.IntTo$Snap x)))
    :pattern (($SortWrappers.IntTo$Snap x))
    :qid |$Snap.Int|
    )))
; Declaring additional sort wrappers
(declare-fun $SortWrappers.$Seq<Int>To$Snap ($Seq<Int>) $Snap)
(declare-fun $SortWrappers.$SnapTo$Seq<Int> ($Snap) $Seq<Int>)
(assert (forall ((x $Seq<Int>)) (!
    (= x ($SortWrappers.$SnapTo$Seq<Int>($SortWrappers.$Seq<Int>To$Snap x)))
    :pattern (($SortWrappers.$Seq<Int>To$Snap x))
    :qid |$Snap.$Seq<Int>|
    )))
; Declaring additional sort wrappers
(declare-fun $SortWrappers.$FVF<$Ref>To$Snap ($FVF<$Ref>) $Snap)
(declare-fun $SortWrappers.$SnapTo$FVF<$Ref> ($Snap) $FVF<$Ref>)
(assert (forall ((x $FVF<$Ref>)) (!
    (= x ($SortWrappers.$SnapTo$FVF<$Ref>($SortWrappers.$FVF<$Ref>To$Snap x)))
    :pattern (($SortWrappers.$FVF<$Ref>To$Snap x))
    :qid |$Snap.$FVF<$Ref>|
    )))
; Declaring additional sort wrappers
(declare-fun $SortWrappers.$PSF<$Snap>To$Snap ($PSF<$Snap>) $Snap)
(declare-fun $SortWrappers.$SnapTo$PSF<$Snap> ($Snap) $PSF<$Snap>)
(assert (forall ((x $PSF<$Snap>)) (!
    (= x ($SortWrappers.$SnapTo$PSF<$Snap>($SortWrappers.$PSF<$Snap>To$Snap x)))
    :pattern (($SortWrappers.$PSF<$Snap>To$Snap x))
    :qid |$Snap.$PSF<$Snap>|
    )))
; Preamble end
; ------------------------------------------------------------
; ---------- SortedList_ctor ----------
(declare-const this@0 $Ref)
(push) ; 1
(push) ; 2
(pop) ; 2
(push) ; 2
; [exec]
; this := SortedList_init()
(declare-const this@1 $Ref)
(assert (not (= this@1 $Ref.null)))
(declare-const $t@2 $Seq<Int>)
; [exec]
; this.SortedList_Elements := Seq[Int]()
; [eval] Seq[Int]()
(pop) ; 2
(pop) ; 1
; ---------- SortedList_Insert ----------
(declare-const this@3 $Ref)
(declare-const element@4 Int)
(declare-const res@5 Int)
(declare-const index@6 Int)
(push) ; 1
(declare-const $t@7 $Snap)
(declare-const $t@8 $Seq<Int>)
(assert (= $t@7 ($Snap.combine ($SortWrappers.$Seq<Int>To$Snap $t@8) $Snap.unit)))
(assert (not (= this@3 $Ref.null)))
; [eval] (forall i: Int :: (forall j: Int :: 0 <= i && (i < j && j < |this.SortedList_Elements|) ==> this.SortedList_Elements[i] <= this.SortedList_Elements[j]))
(declare-const i@9 Int)
(push) ; 2
; [eval] (forall j: Int :: 0 <= i && (i < j && j < |this.SortedList_Elements|) ==> this.SortedList_Elements[i] <= this.SortedList_Elements[j])
(declare-const j@10 Int)
(push) ; 3
; [eval] 0 <= i && (i < j && j < |this.SortedList_Elements|) ==> this.SortedList_Elements[i] <= this.SortedList_Elements[j]
; [eval] 0 <= i && (i < j && j < |this.SortedList_Elements|)
; [eval] 0 <= i
; [eval] 0 <= i ==> i < j && j < |this.SortedList_Elements|
; [eval] 0 <= i
(push) ; 4
(set-option :timeout 250)
(push) ; 5
(assert (not (not (<= 0 i@9))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= 0 i@9)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 0] 0 <= i@9
(assert (<= 0 i@9))
; [eval] i < j && j < |this.SortedList_Elements|
; [eval] i < j
; [eval] i < j ==> j < |this.SortedList_Elements|
; [eval] i < j
(push) ; 6
(push) ; 7
(assert (not (not (< i@9 j@10))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (< i@9 j@10)))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 1] i@9 < j@10
(assert (< i@9 j@10))
; [eval] j < |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
(pop) ; 7
(push) ; 7
; [else-branch 1] !i@9 < j@10
(assert (not (< i@9 j@10)))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(pop) ; 5
(push) ; 5
; [else-branch 0] !0 <= i@9
(assert (not (<= 0 i@9)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(push) ; 4
(push) ; 5
(assert (not (not
  (and
    (<= 0 i@9)
    (implies
      (<= 0 i@9)
      (and (< i@9 j@10) (implies (< i@9 j@10) (< j@10 ($Seq.length $t@8)))))))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (and
  (<= 0 i@9)
  (implies
    (<= 0 i@9)
    (and (< i@9 j@10) (implies (< i@9 j@10) (< j@10 ($Seq.length $t@8))))))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 2] 0 <= i@9 && 0 <= i@9 ==> i@9 < j@10 && i@9 < j@10 ==> j@10 < |$t@8|
(assert (and
  (<= 0 i@9)
  (implies
    (<= 0 i@9)
    (and (< i@9 j@10) (implies (< i@9 j@10) (< j@10 ($Seq.length $t@8)))))))
; [eval] this.SortedList_Elements[i] <= this.SortedList_Elements[j]
; [eval] this.SortedList_Elements[i]
; [eval] this.SortedList_Elements[j]
(pop) ; 5
(push) ; 5
; [else-branch 2] !0 <= i@9 && 0 <= i@9 ==> i@9 < j@10 && i@9 < j@10 ==> j@10 < |$t@8|
(assert (not
  (and
    (<= 0 i@9)
    (implies
      (<= 0 i@9)
      (and (< i@9 j@10) (implies (< i@9 j@10) (< j@10 ($Seq.length $t@8))))))))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(pop) ; 3
; Nested auxiliary terms
(pop) ; 2
; Nested auxiliary terms
(assert (forall ((i@9 Int)) (!
  (forall ((j@10 Int)) (!
    (implies
      (and
        (<= 0 i@9)
        (implies
          (<= 0 i@9)
          (and (< i@9 j@10) (implies (< i@9 j@10) (< j@10 ($Seq.length $t@8))))))
      (<= ($Seq.index $t@8 i@9) ($Seq.index $t@8 j@10)))
    :pattern (($Seq.index $t@8 j@10))
    :qid |prog.l6|))
  :pattern (($Seq.index $t@8 i@9))
  :qid |prog.l6|)))
(push) ; 2
(declare-const $t@11 $Snap)
(declare-const $t@12 $Snap)
(assert (= $t@11 ($Snap.combine $t@12 $Snap.unit)))
(declare-const $t@13 $Seq<Int>)
(assert (= $t@12 ($Snap.combine ($SortWrappers.$Seq<Int>To$Snap $t@13) $Snap.unit)))
; [eval] (forall i2: Int :: (forall j2: Int :: 0 <= i2 && (i2 < j2 && j2 < |this.SortedList_Elements|) ==> this.SortedList_Elements[i2] <= this.SortedList_Elements[j2]))
(declare-const i2@14 Int)
(push) ; 3
; [eval] (forall j2: Int :: 0 <= i2 && (i2 < j2 && j2 < |this.SortedList_Elements|) ==> this.SortedList_Elements[i2] <= this.SortedList_Elements[j2])
(declare-const j2@15 Int)
(push) ; 4
; [eval] 0 <= i2 && (i2 < j2 && j2 < |this.SortedList_Elements|) ==> this.SortedList_Elements[i2] <= this.SortedList_Elements[j2]
; [eval] 0 <= i2 && (i2 < j2 && j2 < |this.SortedList_Elements|)
; [eval] 0 <= i2
; [eval] 0 <= i2 ==> i2 < j2 && j2 < |this.SortedList_Elements|
; [eval] 0 <= i2
(push) ; 5
(push) ; 6
(assert (not (not (<= 0 i2@14))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
(assert (not (<= 0 i2@14)))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
; [then-branch 3] 0 <= i2@14
(assert (<= 0 i2@14))
; [eval] i2 < j2 && j2 < |this.SortedList_Elements|
; [eval] i2 < j2
; [eval] i2 < j2 ==> j2 < |this.SortedList_Elements|
; [eval] i2 < j2
(push) ; 7
(push) ; 8
(assert (not (not (< i2@14 j2@15))))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
(assert (not (< i2@14 j2@15)))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
; [then-branch 4] i2@14 < j2@15
(assert (< i2@14 j2@15))
; [eval] j2 < |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
(pop) ; 8
(push) ; 8
; [else-branch 4] !i2@14 < j2@15
(assert (not (< i2@14 j2@15)))
(pop) ; 8
(pop) ; 7
; Joined path conditions
; Joined path conditions
(pop) ; 6
(push) ; 6
; [else-branch 3] !0 <= i2@14
(assert (not (<= 0 i2@14)))
(pop) ; 6
(pop) ; 5
; Joined path conditions
; Joined path conditions
(push) ; 5
(push) ; 6
(assert (not (not
  (and
    (<= 0 i2@14)
    (implies
      (<= 0 i2@14)
      (and
        (< i2@14 j2@15)
        (implies (< i2@14 j2@15) (< j2@15 ($Seq.length $t@13)))))))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
(assert (not (and
  (<= 0 i2@14)
  (implies
    (<= 0 i2@14)
    (and (< i2@14 j2@15) (implies (< i2@14 j2@15) (< j2@15 ($Seq.length $t@13))))))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
; [then-branch 5] 0 <= i2@14 && 0 <= i2@14 ==> i2@14 < j2@15 && i2@14 < j2@15 ==> j2@15 < |$t@13|
(assert (and
  (<= 0 i2@14)
  (implies
    (<= 0 i2@14)
    (and (< i2@14 j2@15) (implies (< i2@14 j2@15) (< j2@15 ($Seq.length $t@13)))))))
; [eval] this.SortedList_Elements[i2] <= this.SortedList_Elements[j2]
; [eval] this.SortedList_Elements[i2]
; [eval] this.SortedList_Elements[j2]
(pop) ; 6
(push) ; 6
; [else-branch 5] !0 <= i2@14 && 0 <= i2@14 ==> i2@14 < j2@15 && i2@14 < j2@15 ==> j2@15 < |$t@13|
(assert (not
  (and
    (<= 0 i2@14)
    (implies
      (<= 0 i2@14)
      (and
        (< i2@14 j2@15)
        (implies (< i2@14 j2@15) (< j2@15 ($Seq.length $t@13))))))))
(pop) ; 6
(pop) ; 5
; Joined path conditions
; Joined path conditions
(pop) ; 4
; Nested auxiliary terms
(pop) ; 3
; Nested auxiliary terms
(assert (forall ((i2@14 Int)) (!
  (forall ((j2@15 Int)) (!
    (implies
      (and
        (<= 0 i2@14)
        (implies
          (<= 0 i2@14)
          (and
            (< i2@14 j2@15)
            (implies (< i2@14 j2@15) (< j2@15 ($Seq.length $t@13))))))
      (<= ($Seq.index $t@13 i2@14) ($Seq.index $t@13 j2@15)))
    :pattern (($Seq.index $t@13 j2@15))
    :qid |prog.l7|))
  :pattern (($Seq.index $t@13 i2@14))
  :qid |prog.l7|)))
; [eval] 0 <= res && res <= old(|this.SortedList_Elements|)
; [eval] 0 <= res
; [eval] 0 <= res ==> res <= old(|this.SortedList_Elements|)
; [eval] 0 <= res
(push) ; 3
(push) ; 4
(assert (not (not (<= 0 res@5))))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
(assert (not (<= 0 res@5)))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
; [then-branch 6] 0 <= res@5
(assert (<= 0 res@5))
; [eval] res <= old(|this.SortedList_Elements|)
; [eval] old(|this.SortedList_Elements|)
; [eval] |this.SortedList_Elements|
(pop) ; 4
(push) ; 4
; [else-branch 6] !0 <= res@5
(assert (not (<= 0 res@5)))
(pop) ; 4
(pop) ; 3
; Joined path conditions
; Joined path conditions
(assert (and (<= 0 res@5) (implies (<= 0 res@5) (<= res@5 ($Seq.length $t@8)))))
; [eval] this.SortedList_Elements == old(this.SortedList_Elements)[..res] ++ Seq(element) ++ old(this.SortedList_Elements)[res..]
; [eval] old(this.SortedList_Elements)[..res] ++ Seq(element) ++ old(this.SortedList_Elements)[res..]
; [eval] old(this.SortedList_Elements)[..res] ++ Seq(element)
; [eval] old(this.SortedList_Elements)[..res]
; [eval] old(this.SortedList_Elements)
; [eval] Seq(element)
(assert (= ($Seq.length ($Seq.singleton element@4)) 1))
; [eval] old(this.SortedList_Elements)[res..]
; [eval] old(this.SortedList_Elements)
(assert ($Seq.equal
  $t@13
  ($Seq.append
    ($Seq.append ($Seq.take $t@8 res@5) ($Seq.singleton element@4))
    ($Seq.drop $t@8 res@5))))
(pop) ; 2
(push) ; 2
; [exec]
; index := 0
; loop at tmp2078.tmp@13.2
(declare-const index@16 Int)
(push) ; 3
; Loop: Check specs well-definedness
(declare-const $t@17 $Snap)
(declare-const $t@18 $Seq<Int>)
(assert (= $t@17 ($Snap.combine ($SortWrappers.$Seq<Int>To$Snap $t@18) $Snap.unit)))
(set-option :timeout 0)
(push) ; 4
(assert (not (not (= 2 0))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (implies (< $Perm.No (/ (to_real 1) (to_real 2))) (not (= this@3 $Ref.null))))
; [eval] 0 <= index && index <= |this.SortedList_Elements|
; [eval] 0 <= index
; [eval] 0 <= index ==> index <= |this.SortedList_Elements|
; [eval] 0 <= index
(push) ; 4
(set-option :timeout 250)
(push) ; 5
(assert (not (not (<= 0 index@16))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= 0 index@16)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 7] 0 <= index@16
(assert (<= 0 index@16))
; [eval] index <= |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
(pop) ; 5
(push) ; 5
; [else-branch 7] !0 <= index@16
(assert (not (<= 0 index@16)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(assert (and (<= 0 index@16) (implies (<= 0 index@16) (<= index@16 ($Seq.length $t@18)))))
; [eval] (forall i3: Int :: 0 <= i3 && i3 < index ==> this.SortedList_Elements[i3] < element)
(declare-const i3@19 Int)
(push) ; 4
; [eval] 0 <= i3 && i3 < index ==> this.SortedList_Elements[i3] < element
; [eval] 0 <= i3 && i3 < index
; [eval] 0 <= i3
; [eval] 0 <= i3 ==> i3 < index
; [eval] 0 <= i3
(push) ; 5
(push) ; 6
(assert (not (not (<= 0 i3@19))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
(assert (not (<= 0 i3@19)))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
; [then-branch 8] 0 <= i3@19
(assert (<= 0 i3@19))
; [eval] i3 < index
(pop) ; 6
(push) ; 6
; [else-branch 8] !0 <= i3@19
(assert (not (<= 0 i3@19)))
(pop) ; 6
(pop) ; 5
; Joined path conditions
; Joined path conditions
(push) ; 5
(push) ; 6
(assert (not (not (and (<= 0 i3@19) (implies (<= 0 i3@19) (< i3@19 index@16))))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
(assert (not (and (<= 0 i3@19) (implies (<= 0 i3@19) (< i3@19 index@16)))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
; [then-branch 9] 0 <= i3@19 && 0 <= i3@19 ==> i3@19 < index@16
(assert (and (<= 0 i3@19) (implies (<= 0 i3@19) (< i3@19 index@16))))
; [eval] this.SortedList_Elements[i3] < element
; [eval] this.SortedList_Elements[i3]
(pop) ; 6
(push) ; 6
; [else-branch 9] !0 <= i3@19 && 0 <= i3@19 ==> i3@19 < index@16
(assert (not (and (<= 0 i3@19) (implies (<= 0 i3@19) (< i3@19 index@16)))))
(pop) ; 6
(pop) ; 5
; Joined path conditions
; Joined path conditions
(pop) ; 4
; Nested auxiliary terms
(assert (forall ((i3@19 Int)) (!
  (implies
    (and (<= 0 i3@19) (implies (<= 0 i3@19) (< i3@19 index@16)))
    (< ($Seq.index $t@18 i3@19) element@4))
  :pattern (($Seq.index $t@18 i3@19))
  :qid |prog.l16|)))
(declare-const $t@20 $Snap)
(assert (= $t@20 $Snap.unit))
; [eval] index < |this.SortedList_Elements| && this.SortedList_Elements[index] < element
; [eval] index < |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
; [eval] index < |this.SortedList_Elements| ==> this.SortedList_Elements[index] < element
; [eval] index < |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
(push) ; 4
(push) ; 5
(assert (not (not (< index@16 ($Seq.length $t@18)))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (< index@16 ($Seq.length $t@18))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 10] index@16 < |$t@18|
(assert (< index@16 ($Seq.length $t@18)))
; [eval] this.SortedList_Elements[index] < element
; [eval] this.SortedList_Elements[index]
(pop) ; 5
(push) ; 5
; [else-branch 10] !index@16 < |$t@18|
(assert (not (< index@16 ($Seq.length $t@18))))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(assert (and
  (< index@16 ($Seq.length $t@18))
  (implies
    (< index@16 ($Seq.length $t@18))
    (< ($Seq.index $t@18 index@16) element@4))))
(check-sat)
; unknown
(pop) ; 3
(push) ; 3
; Loop: Establish loop invariant
(set-option :timeout 0)
(push) ; 4
(assert (not (not (= 2 0))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
(assert (not (<= (/ (to_real 1) (to_real 2)) $Perm.Write)))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (<= (/ (to_real 1) (to_real 2)) $Perm.Write))
(set-option :timeout 250)
(push) ; 4
(assert (not (or
  (= (- $Perm.Write (/ (to_real 1) (to_real 2))) $Perm.No)
  (< (- $Perm.Write (/ (to_real 1) (to_real 2))) $Perm.No))))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
; [eval] 0 <= index && index <= |this.SortedList_Elements|
; [eval] 0 <= index
; [eval] 0 <= index ==> index <= |this.SortedList_Elements|
; [eval] 0 <= index
(push) ; 4
(push) ; 5
(assert (not false))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 11] True
; [eval] index <= |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
(pop) ; 5
; [dead else-branch 11] False
(pop) ; 4
; Joined path conditions
(set-option :timeout 0)
(push) ; 4
(assert (not (<= 0 ($Seq.length $t@8))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (<= 0 ($Seq.length $t@8)))
; [eval] (forall i3: Int :: 0 <= i3 && i3 < index ==> this.SortedList_Elements[i3] < element)
(declare-const i3@21 Int)
(push) ; 4
; [eval] 0 <= i3 && i3 < index ==> this.SortedList_Elements[i3] < element
; [eval] 0 <= i3 && i3 < index
; [eval] 0 <= i3
; [eval] 0 <= i3 ==> i3 < index
; [eval] 0 <= i3
(push) ; 5
(set-option :timeout 250)
(push) ; 6
(assert (not (not (<= 0 i3@21))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
(assert (not (<= 0 i3@21)))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
; [then-branch 12] 0 <= i3@21
(assert (<= 0 i3@21))
; [eval] i3 < index
(pop) ; 6
(push) ; 6
; [else-branch 12] !0 <= i3@21
(assert (not (<= 0 i3@21)))
(pop) ; 6
(pop) ; 5
; Joined path conditions
; Joined path conditions
(push) ; 5
(push) ; 6
(assert (not (not (and (<= 0 i3@21) (implies (<= 0 i3@21) (< i3@21 0))))))
(check-sat)
; unsat
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
; [dead then-branch 13] 0 <= i3@21 && 0 <= i3@21 ==> i3@21 < 0
(push) ; 6
; [else-branch 13] !0 <= i3@21 && 0 <= i3@21 ==> i3@21 < 0
(assert (not (and (<= 0 i3@21) (implies (<= 0 i3@21) (< i3@21 0)))))
(pop) ; 6
(pop) ; 5
; Joined path conditions
; [eval] this.SortedList_Elements[i3]
(pop) ; 4
; Nested auxiliary terms
(pop) ; 3
; Loop: Verify loop body
(push) ; 3
(assert (and
  (< index@16 ($Seq.length $t@18))
  (implies
    (< index@16 ($Seq.length $t@18))
    (< ($Seq.index $t@18 index@16) element@4))))
(assert (= $t@20 $Snap.unit))
(assert (forall ((i3@19 Int)) (!
  (implies
    (and (<= 0 i3@19) (implies (<= 0 i3@19) (< i3@19 index@16)))
    (< ($Seq.index $t@18 i3@19) element@4))
  :pattern (($Seq.index $t@18 i3@19))
  :qid |prog.l16|)))
(assert (and (<= 0 index@16) (implies (<= 0 index@16) (<= index@16 ($Seq.length $t@18)))))
(assert (implies (< $Perm.No (/ (to_real 1) (to_real 2))) (not (= this@3 $Ref.null))))
(assert (= $t@17 ($Snap.combine ($SortWrappers.$Seq<Int>To$Snap $t@18) $Snap.unit)))
; [exec]
; index := index + 1
; [eval] index + 1
(declare-const index@22 Int)
(assert (= index@22 (+ index@16 1)))
(set-option :timeout 0)
(push) ; 4
(assert (not (not (= 2 0))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
; [eval] 0 <= index && index <= |this.SortedList_Elements|
; [eval] 0 <= index
; [eval] 0 <= index ==> index <= |this.SortedList_Elements|
; [eval] 0 <= index
(push) ; 4
(set-option :timeout 250)
(push) ; 5
(assert (not (not (<= 0 index@22))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= 0 index@22)))
(check-sat)
; unsat
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 14] 0 <= index@22
(assert (<= 0 index@22))
; [eval] index <= |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
(pop) ; 5
; [dead else-branch 14] !0 <= index@22
(pop) ; 4
; Joined path conditions
(set-option :timeout 0)
(push) ; 4
(assert (not (and (<= 0 index@22) (implies (<= 0 index@22) (<= index@22 ($Seq.length $t@18))))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (and (<= 0 index@22) (implies (<= 0 index@22) (<= index@22 ($Seq.length $t@18)))))
; [eval] (forall i3: Int :: 0 <= i3 && i3 < index ==> this.SortedList_Elements[i3] < element)
(declare-const i3@23 Int)
(push) ; 4
; [eval] 0 <= i3 && i3 < index ==> this.SortedList_Elements[i3] < element
; [eval] 0 <= i3 && i3 < index
; [eval] 0 <= i3
; [eval] 0 <= i3 ==> i3 < index
; [eval] 0 <= i3
(push) ; 5
(set-option :timeout 250)
(push) ; 6
(assert (not (not (<= 0 i3@23))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
(assert (not (<= 0 i3@23)))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
; [then-branch 15] 0 <= i3@23
(assert (<= 0 i3@23))
; [eval] i3 < index
(pop) ; 6
(push) ; 6
; [else-branch 15] !0 <= i3@23
(assert (not (<= 0 i3@23)))
(pop) ; 6
(pop) ; 5
; Joined path conditions
; Joined path conditions
(push) ; 5
(push) ; 6
(assert (not (not (and (<= 0 i3@23) (implies (<= 0 i3@23) (< i3@23 index@22))))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
(assert (not (and (<= 0 i3@23) (implies (<= 0 i3@23) (< i3@23 index@22)))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
; [then-branch 16] 0 <= i3@23 && 0 <= i3@23 ==> i3@23 < index@22
(assert (and (<= 0 i3@23) (implies (<= 0 i3@23) (< i3@23 index@22))))
; [eval] this.SortedList_Elements[i3] < element
; [eval] this.SortedList_Elements[i3]
(pop) ; 6
(push) ; 6
; [else-branch 16] !0 <= i3@23 && 0 <= i3@23 ==> i3@23 < index@22
(assert (not (and (<= 0 i3@23) (implies (<= 0 i3@23) (< i3@23 index@22)))))
(pop) ; 6
(pop) ; 5
; Joined path conditions
; Joined path conditions
(pop) ; 4
; Nested auxiliary terms
(set-option :timeout 0)
(push) ; 4
(assert (not (forall ((i3@23 Int)) (!
  (implies
    (and (<= 0 i3@23) (implies (<= 0 i3@23) (< i3@23 index@22)))
    (< ($Seq.index $t@18 i3@23) element@4))
  :pattern (($Seq.index $t@18 i3@23))
  :qid |prog.l16|))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (forall ((i3@23 Int)) (!
  (implies
    (and (<= 0 i3@23) (implies (<= 0 i3@23) (< i3@23 index@22)))
    (< ($Seq.index $t@18 i3@23) element@4))
  :pattern (($Seq.index $t@18 i3@23))
  :qid |prog.l16|)))
(pop) ; 3
; Loop: Continue after loop
(push) ; 3
(assert (<= 0 ($Seq.length $t@8)))
(assert (<= (/ (to_real 1) (to_real 2)) $Perm.Write))
(declare-const $t@24 $Snap)
(declare-const $t@25 $Seq<Int>)
(assert (= $t@24 ($Snap.combine ($SortWrappers.$Seq<Int>To$Snap $t@25) $Snap.unit)))
(push) ; 4
(assert (not (not (= 2 0))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (implies (< $Perm.No (/ (to_real 1) (to_real 2))) (not (= this@3 $Ref.null))))
(assert (implies
  (< $Perm.No (- $Perm.Write (/ (to_real 1) (to_real 2))))
  ($Seq.equal $t@25 $t@8)))
; [eval] 0 <= index && index <= |this.SortedList_Elements|
; [eval] 0 <= index
; [eval] 0 <= index ==> index <= |this.SortedList_Elements|
; [eval] 0 <= index
(push) ; 4
(set-option :timeout 250)
(push) ; 5
(assert (not (not (<= 0 index@16))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= 0 index@16)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 17] 0 <= index@16
(assert (<= 0 index@16))
; [eval] index <= |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
(pop) ; 5
(push) ; 5
; [else-branch 17] !0 <= index@16
(assert (not (<= 0 index@16)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(assert (and (<= 0 index@16) (implies (<= 0 index@16) (<= index@16 ($Seq.length $t@25)))))
; [eval] (forall i3: Int :: 0 <= i3 && i3 < index ==> this.SortedList_Elements[i3] < element)
(declare-const i3@26 Int)
(push) ; 4
; [eval] 0 <= i3 && i3 < index ==> this.SortedList_Elements[i3] < element
; [eval] 0 <= i3 && i3 < index
; [eval] 0 <= i3
; [eval] 0 <= i3 ==> i3 < index
; [eval] 0 <= i3
(push) ; 5
(push) ; 6
(assert (not (not (<= 0 i3@26))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
(assert (not (<= 0 i3@26)))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
; [then-branch 18] 0 <= i3@26
(assert (<= 0 i3@26))
; [eval] i3 < index
(pop) ; 6
(push) ; 6
; [else-branch 18] !0 <= i3@26
(assert (not (<= 0 i3@26)))
(pop) ; 6
(pop) ; 5
; Joined path conditions
; Joined path conditions
(push) ; 5
(push) ; 6
(assert (not (not (and (<= 0 i3@26) (implies (<= 0 i3@26) (< i3@26 index@16))))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
(assert (not (and (<= 0 i3@26) (implies (<= 0 i3@26) (< i3@26 index@16)))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
; [then-branch 19] 0 <= i3@26 && 0 <= i3@26 ==> i3@26 < index@16
(assert (and (<= 0 i3@26) (implies (<= 0 i3@26) (< i3@26 index@16))))
; [eval] this.SortedList_Elements[i3] < element
; [eval] this.SortedList_Elements[i3]
(pop) ; 6
(push) ; 6
; [else-branch 19] !0 <= i3@26 && 0 <= i3@26 ==> i3@26 < index@16
(assert (not (and (<= 0 i3@26) (implies (<= 0 i3@26) (< i3@26 index@16)))))
(pop) ; 6
(pop) ; 5
; Joined path conditions
; Joined path conditions
(pop) ; 4
; Nested auxiliary terms
(assert (forall ((i3@26 Int)) (!
  (implies
    (and (<= 0 i3@26) (implies (<= 0 i3@26) (< i3@26 index@16)))
    (< ($Seq.index $t@25 i3@26) element@4))
  :pattern (($Seq.index $t@25 i3@26))
  :qid |prog.l16|)))
; [eval] !(index < |this.SortedList_Elements| && this.SortedList_Elements[index] < element)
; [eval] index < |this.SortedList_Elements| && this.SortedList_Elements[index] < element
; [eval] index < |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
; [eval] index < |this.SortedList_Elements| ==> this.SortedList_Elements[index] < element
; [eval] index < |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
(push) ; 4
(push) ; 5
(assert (not (not (< index@16 ($Seq.length $t@25)))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (< index@16 ($Seq.length $t@25))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 20] index@16 < |$t@25|
(assert (< index@16 ($Seq.length $t@25)))
; [eval] this.SortedList_Elements[index] < element
; [eval] this.SortedList_Elements[index]
(pop) ; 5
(push) ; 5
; [else-branch 20] !index@16 < |$t@25|
(assert (not (< index@16 ($Seq.length $t@25))))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(assert (not
  (and
    (< index@16 ($Seq.length $t@25))
    (implies
      (< index@16 ($Seq.length $t@25))
      (< ($Seq.index $t@25 index@16) element@4)))))
(check-sat)
; unknown
; [exec]
; this.SortedList_Elements := this.SortedList_Elements[..index] ++ Seq(element) ++ this.SortedList_Elements[index..]
; [eval] this.SortedList_Elements[..index] ++ Seq(element) ++ this.SortedList_Elements[index..]
; [eval] this.SortedList_Elements[..index] ++ Seq(element)
; [eval] this.SortedList_Elements[..index]
; [eval] Seq(element)
(assert (= ($Seq.length ($Seq.singleton element@4)) 1))
; [eval] this.SortedList_Elements[index..]
; [exec]
; res := index
; [eval] (forall i2: Int :: (forall j2: Int :: 0 <= i2 && (i2 < j2 && j2 < |this.SortedList_Elements|) ==> this.SortedList_Elements[i2] <= this.SortedList_Elements[j2]))
(declare-const i2@27 Int)
(push) ; 4
; [eval] (forall j2: Int :: 0 <= i2 && (i2 < j2 && j2 < |this.SortedList_Elements|) ==> this.SortedList_Elements[i2] <= this.SortedList_Elements[j2])
(declare-const j2@28 Int)
(push) ; 5
; [eval] 0 <= i2 && (i2 < j2 && j2 < |this.SortedList_Elements|) ==> this.SortedList_Elements[i2] <= this.SortedList_Elements[j2]
; [eval] 0 <= i2 && (i2 < j2 && j2 < |this.SortedList_Elements|)
; [eval] 0 <= i2
; [eval] 0 <= i2 ==> i2 < j2 && j2 < |this.SortedList_Elements|
; [eval] 0 <= i2
(push) ; 6
(push) ; 7
(assert (not (not (<= 0 i2@27))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (<= 0 i2@27)))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 21] 0 <= i2@27
(assert (<= 0 i2@27))
; [eval] i2 < j2 && j2 < |this.SortedList_Elements|
; [eval] i2 < j2
; [eval] i2 < j2 ==> j2 < |this.SortedList_Elements|
; [eval] i2 < j2
(push) ; 8
(push) ; 9
(assert (not (not (< i2@27 j2@28))))
(check-sat)
; unknown
(pop) ; 9
; 0,00s
; (get-info :all-statistics)
(push) ; 9
(assert (not (< i2@27 j2@28)))
(check-sat)
; unknown
(pop) ; 9
; 0,00s
; (get-info :all-statistics)
(push) ; 9
; [then-branch 22] i2@27 < j2@28
(assert (< i2@27 j2@28))
; [eval] j2 < |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
(pop) ; 9
(push) ; 9
; [else-branch 22] !i2@27 < j2@28
(assert (not (< i2@27 j2@28)))
(pop) ; 9
(pop) ; 8
; Joined path conditions
; Joined path conditions
(pop) ; 7
(push) ; 7
; [else-branch 21] !0 <= i2@27
(assert (not (<= 0 i2@27)))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(push) ; 6
(push) ; 7
(assert (not (not
  (and
    (<= 0 i2@27)
    (implies
      (<= 0 i2@27)
      (and
        (< i2@27 j2@28)
        (implies
          (< i2@27 j2@28)
          (<
            j2@28
            ($Seq.length
              ($Seq.append
                ($Seq.append
                  ($Seq.take $t@25 index@16)
                  ($Seq.singleton element@4))
                ($Seq.drop $t@25 index@16)))))))))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (and
  (<= 0 i2@27)
  (implies
    (<= 0 i2@27)
    (and
      (< i2@27 j2@28)
      (implies
        (< i2@27 j2@28)
        (<
          j2@28
          ($Seq.length
            ($Seq.append
              ($Seq.append ($Seq.take $t@25 index@16) ($Seq.singleton element@4))
              ($Seq.drop $t@25 index@16))))))))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 23] 0 <= i2@27 && 0 <= i2@27 ==> i2@27 < j2@28 && i2@27 < j2@28 ==> j2@28 < |$t@25[:index@16] ++ [element@4] ++ $t@25[index@16:]|
(assert (and
  (<= 0 i2@27)
  (implies
    (<= 0 i2@27)
    (and
      (< i2@27 j2@28)
      (implies
        (< i2@27 j2@28)
        (<
          j2@28
          ($Seq.length
            ($Seq.append
              ($Seq.append ($Seq.take $t@25 index@16) ($Seq.singleton element@4))
              ($Seq.drop $t@25 index@16)))))))))
; [eval] this.SortedList_Elements[i2] <= this.SortedList_Elements[j2]
; [eval] this.SortedList_Elements[i2]
; [eval] this.SortedList_Elements[j2]
(pop) ; 7
(push) ; 7
; [else-branch 23] !0 <= i2@27 && 0 <= i2@27 ==> i2@27 < j2@28 && i2@27 < j2@28 ==> j2@28 < |$t@25[:index@16] ++ [element@4] ++ $t@25[index@16:]|
(assert (not
  (and
    (<= 0 i2@27)
    (implies
      (<= 0 i2@27)
      (and
        (< i2@27 j2@28)
        (implies
          (< i2@27 j2@28)
          (<
            j2@28
            ($Seq.length
              ($Seq.append
                ($Seq.append
                  ($Seq.take $t@25 index@16)
                  ($Seq.singleton element@4))
                ($Seq.drop $t@25 index@16))))))))))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(pop) ; 5
; Nested auxiliary terms
(pop) ; 4
; Nested auxiliary terms
(set-option :timeout 0)
(push) ; 4
(assert (not (forall ((i2@27 Int)) (!
  (forall ((j2@28 Int)) (!
    (implies
      (and
        (<= 0 i2@27)
        (implies
          (<= 0 i2@27)
          (and
            (< i2@27 j2@28)
            (implies
              (< i2@27 j2@28)
              (<
                j2@28
                ($Seq.length
                  ($Seq.append
                    ($Seq.append
                      ($Seq.take $t@25 index@16)
                      ($Seq.singleton element@4))
                    ($Seq.drop $t@25 index@16))))))))
      (<=
        ($Seq.index
          ($Seq.append
            ($Seq.append ($Seq.take $t@25 index@16) ($Seq.singleton element@4))
            ($Seq.drop $t@25 index@16))
          i2@27)
        ($Seq.index
          ($Seq.append
            ($Seq.append ($Seq.take $t@25 index@16) ($Seq.singleton element@4))
            ($Seq.drop $t@25 index@16))
          j2@28)))
    :pattern (($Seq.index
      ($Seq.append
        ($Seq.append ($Seq.take $t@25 index@16) ($Seq.singleton element@4))
        ($Seq.drop $t@25 index@16))
      j2@28))
    :qid |prog.l7|))
  :pattern (($Seq.index
    ($Seq.append
      ($Seq.append ($Seq.take $t@25 index@16) ($Seq.singleton element@4))
      ($Seq.drop $t@25 index@16))
    i2@27))
  :qid |prog.l7|))))
(check-sat)
; unsat
(pop) ; 4
; 0,03s
; (get-info :all-statistics)
(assert (forall ((i2@27 Int)) (!
  (forall ((j2@28 Int)) (!
    (implies
      (and
        (<= 0 i2@27)
        (implies
          (<= 0 i2@27)
          (and
            (< i2@27 j2@28)
            (implies
              (< i2@27 j2@28)
              (<
                j2@28
                ($Seq.length
                  ($Seq.append
                    ($Seq.append
                      ($Seq.take $t@25 index@16)
                      ($Seq.singleton element@4))
                    ($Seq.drop $t@25 index@16))))))))
      (<=
        ($Seq.index
          ($Seq.append
            ($Seq.append ($Seq.take $t@25 index@16) ($Seq.singleton element@4))
            ($Seq.drop $t@25 index@16))
          i2@27)
        ($Seq.index
          ($Seq.append
            ($Seq.append ($Seq.take $t@25 index@16) ($Seq.singleton element@4))
            ($Seq.drop $t@25 index@16))
          j2@28)))
    :pattern (($Seq.index
      ($Seq.append
        ($Seq.append ($Seq.take $t@25 index@16) ($Seq.singleton element@4))
        ($Seq.drop $t@25 index@16))
      j2@28))
    :qid |prog.l7|))
  :pattern (($Seq.index
    ($Seq.append
      ($Seq.append ($Seq.take $t@25 index@16) ($Seq.singleton element@4))
      ($Seq.drop $t@25 index@16))
    i2@27))
  :qid |prog.l7|)))
; [eval] 0 <= res && res <= old(|this.SortedList_Elements|)
; [eval] 0 <= res
; [eval] 0 <= res ==> res <= old(|this.SortedList_Elements|)
; [eval] 0 <= res
(push) ; 4
(set-option :timeout 250)
(push) ; 5
(assert (not (not (<= 0 index@16))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= 0 index@16)))
(check-sat)
; unsat
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 24] 0 <= index@16
(assert (<= 0 index@16))
; [eval] res <= old(|this.SortedList_Elements|)
; [eval] old(|this.SortedList_Elements|)
; [eval] |this.SortedList_Elements|
(pop) ; 5
; [dead else-branch 24] !0 <= index@16
(pop) ; 4
; Joined path conditions
(set-option :timeout 0)
(push) ; 4
(assert (not (and (<= 0 index@16) (implies (<= 0 index@16) (<= index@16 ($Seq.length $t@8))))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (and (<= 0 index@16) (implies (<= 0 index@16) (<= index@16 ($Seq.length $t@8)))))
; [eval] this.SortedList_Elements == old(this.SortedList_Elements)[..res] ++ Seq(element) ++ old(this.SortedList_Elements)[res..]
; [eval] old(this.SortedList_Elements)[..res] ++ Seq(element) ++ old(this.SortedList_Elements)[res..]
; [eval] old(this.SortedList_Elements)[..res] ++ Seq(element)
; [eval] old(this.SortedList_Elements)[..res]
; [eval] old(this.SortedList_Elements)
; [eval] Seq(element)
; [eval] old(this.SortedList_Elements)[res..]
; [eval] old(this.SortedList_Elements)
(push) ; 4
(assert (not ($Seq.equal
  ($Seq.append
    ($Seq.append ($Seq.take $t@25 index@16) ($Seq.singleton element@4))
    ($Seq.drop $t@25 index@16))
  ($Seq.append
    ($Seq.append ($Seq.take $t@8 index@16) ($Seq.singleton element@4))
    ($Seq.drop $t@8 index@16)))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert ($Seq.equal
  ($Seq.append
    ($Seq.append ($Seq.take $t@25 index@16) ($Seq.singleton element@4))
    ($Seq.drop $t@25 index@16))
  ($Seq.append
    ($Seq.append ($Seq.take $t@8 index@16) ($Seq.singleton element@4))
    ($Seq.drop $t@8 index@16))))
(pop) ; 3
(pop) ; 2
(pop) ; 1
; ---------- SortedList_init ----------
(declare-const this@29 $Ref)
(push) ; 1
(push) ; 2
(assert (not (= this@29 $Ref.null)))
(declare-const $t@30 $Seq<Int>)
(pop) ; 2
(push) ; 2
; [exec]
; this := new(SortedList_Elements, arrayContents)
(declare-const this@31 $Ref)
(assert (not (= this@31 $Ref.null)))
(declare-const SortedList_Elements@32 $Seq<Int>)
(declare-const arrayContents@33 $Seq<Int>)
(pop) ; 2
(pop) ; 1
