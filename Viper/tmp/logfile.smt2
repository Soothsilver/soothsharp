(get-info :version)
; (:version "4.4.1")
; Input file is C:\Users\Soothsilver\AppData\Local\Temp\tmp2078.tmp
; Started: 2017-05-17 20:31:38
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
(assert (not (= this@3 $Ref.null)))
(declare-const $t@7 $Seq<Int>)
(push) ; 2
(declare-const $t@8 $Snap)
(declare-const $t@9 $Seq<Int>)
(assert (= $t@8 ($Snap.combine ($SortWrappers.$Seq<Int>To$Snap $t@9) $Snap.unit)))
; [eval] 0 <= res && res <= old(|this.SortedList_Elements|)
; [eval] 0 <= res
; [eval] 0 <= res ==> res <= old(|this.SortedList_Elements|)
; [eval] 0 <= res
(push) ; 3
(set-option :timeout 250)
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
; [then-branch 0] 0 <= res@5
(assert (<= 0 res@5))
; [eval] res <= old(|this.SortedList_Elements|)
; [eval] old(|this.SortedList_Elements|)
; [eval] |this.SortedList_Elements|
(pop) ; 4
(push) ; 4
; [else-branch 0] !0 <= res@5
(assert (not (<= 0 res@5)))
(pop) ; 4
(pop) ; 3
; Joined path conditions
; Joined path conditions
(assert (and (<= 0 res@5) (implies (<= 0 res@5) (<= res@5 ($Seq.length $t@7)))))
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
  $t@9
  ($Seq.append
    ($Seq.append ($Seq.take $t@7 res@5) ($Seq.singleton element@4))
    ($Seq.drop $t@7 res@5))))
(pop) ; 2
(push) ; 2
; [exec]
; index := 0
; loop at tmp2078.tmp@13.2
(declare-const index@10 Int)
(push) ; 3
; Loop: Check specs well-definedness
(declare-const $t@11 $Snap)
(declare-const $t@12 $Seq<Int>)
(assert (= $t@11 ($Snap.combine ($SortWrappers.$Seq<Int>To$Snap $t@12) $Snap.unit)))
(set-option :timeout 0)
(push) ; 4
(assert (not (not (= 2 0))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (implies (< $Perm.No (/ (to_real 1) (to_real 2))) (not (= this@3 $Ref.null))))
; [eval] 0 <= index && index < |this.SortedList_Elements|
; [eval] 0 <= index
; [eval] 0 <= index ==> index < |this.SortedList_Elements|
; [eval] 0 <= index
(push) ; 4
(set-option :timeout 250)
(push) ; 5
(assert (not (not (<= 0 index@10))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= 0 index@10)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 1] 0 <= index@10
(assert (<= 0 index@10))
; [eval] index < |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
(pop) ; 5
(push) ; 5
; [else-branch 1] !0 <= index@10
(assert (not (<= 0 index@10)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(assert (and (<= 0 index@10) (implies (<= 0 index@10) (< index@10 ($Seq.length $t@12)))))
; [eval] (forall i: Int :: 0 <= i && i < index ==> this.SortedList_Elements[i] < element)
(declare-const i@13 Int)
(push) ; 4
; [eval] 0 <= i && i < index ==> this.SortedList_Elements[i] < element
; [eval] 0 <= i && i < index
; [eval] 0 <= i
; [eval] 0 <= i ==> i < index
; [eval] 0 <= i
(push) ; 5
(push) ; 6
(assert (not (not (<= 0 i@13))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
(assert (not (<= 0 i@13)))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
; [then-branch 2] 0 <= i@13
(assert (<= 0 i@13))
; [eval] i < index
(pop) ; 6
(push) ; 6
; [else-branch 2] !0 <= i@13
(assert (not (<= 0 i@13)))
(pop) ; 6
(pop) ; 5
; Joined path conditions
; Joined path conditions
(push) ; 5
(push) ; 6
(assert (not (not (and (<= 0 i@13) (implies (<= 0 i@13) (< i@13 index@10))))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
(assert (not (and (<= 0 i@13) (implies (<= 0 i@13) (< i@13 index@10)))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
; [then-branch 3] 0 <= i@13 && 0 <= i@13 ==> i@13 < index@10
(assert (and (<= 0 i@13) (implies (<= 0 i@13) (< i@13 index@10))))
; [eval] this.SortedList_Elements[i] < element
; [eval] this.SortedList_Elements[i]
(pop) ; 6
(push) ; 6
; [else-branch 3] !0 <= i@13 && 0 <= i@13 ==> i@13 < index@10
(assert (not (and (<= 0 i@13) (implies (<= 0 i@13) (< i@13 index@10)))))
(pop) ; 6
(pop) ; 5
; Joined path conditions
; Joined path conditions
(pop) ; 4
; Nested auxiliary terms
(assert (forall ((i@13 Int)) (!
  (implies
    (and (<= 0 i@13) (implies (<= 0 i@13) (< i@13 index@10)))
    (< ($Seq.index $t@12 i@13) element@4))
  :pattern (($Seq.index $t@12 i@13))
  :qid |prog.l16|)))
(declare-const $t@14 $Snap)
(assert (= $t@14 $Snap.unit))
; [eval] index < |this.SortedList_Elements| && this.SortedList_Elements[index] < element
; [eval] index < |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
; [eval] index < |this.SortedList_Elements| ==> this.SortedList_Elements[index] < element
; [eval] index < |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
(push) ; 4
(push) ; 5
(assert (not (not (< index@10 ($Seq.length $t@12)))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (< index@10 ($Seq.length $t@12))))
(check-sat)
; unsat
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 4] index@10 < |$t@12|
(assert (< index@10 ($Seq.length $t@12)))
; [eval] this.SortedList_Elements[index] < element
; [eval] this.SortedList_Elements[index]
(pop) ; 5
; [dead else-branch 4] !index@10 < |$t@12|
(pop) ; 4
; Joined path conditions
(assert (and
  (< index@10 ($Seq.length $t@12))
  (implies
    (< index@10 ($Seq.length $t@12))
    (< ($Seq.index $t@12 index@10) element@4))))
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
; [eval] 0 <= index && index < |this.SortedList_Elements|
; [eval] 0 <= index
; [eval] 0 <= index ==> index < |this.SortedList_Elements|
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
; [then-branch 5] True
; [eval] index < |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
(pop) ; 5
; [dead else-branch 5] False
(pop) ; 4
; Joined path conditions
(set-option :timeout 0)
(push) ; 4
(assert (not (< 0 ($Seq.length $t@7))))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
; [eval] 0 <= index && index < |this.SortedList_Elements|
; [eval] 0 <= index
; [eval] 0 <= index ==> index < |this.SortedList_Elements|
; [eval] 0 <= index
(push) ; 4
(set-option :timeout 250)
(push) ; 5
(assert (not false))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 6] True
; [eval] index < |this.SortedList_Elements|
; [eval] |this.SortedList_Elements|
(pop) ; 5
; [dead else-branch 6] False
(pop) ; 4
; Joined path conditions
(set-option :timeout 0)
(push) ; 4
(assert (not (< 0 ($Seq.length $t@7))))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(pop) ; 3
(pop) ; 2
(pop) ; 1
; ---------- SortedList_init ----------
(declare-const this@15 $Ref)
(push) ; 1
(push) ; 2
(assert (not (= this@15 $Ref.null)))
(declare-const $t@16 $Seq<Int>)
(pop) ; 2
(push) ; 2
; [exec]
; this := new(SortedList_Elements, arrayContents)
(declare-const this@17 $Ref)
(assert (not (= this@17 $Ref.null)))
(declare-const SortedList_Elements@18 $Seq<Int>)
(declare-const arrayContents@19 $Seq<Int>)
(pop) ; 2
(pop) ; 1
