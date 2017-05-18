(get-info :version)
; (:version "4.4.1")
; Input file is C:\Users\Soothsilver\AppData\Local\Temp\tmp919D.tmp
; Started: 2017-05-18 08:05:25
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
; ---------- SeqUtils_SeqToArray ----------
(declare-const sequence@0 $Seq<Int>)
(declare-const res@1 $Ref)
(declare-const v@2 $Ref)
(declare-const _tmp1@3 $Ref)
(push) ; 1
(push) ; 2
(pop) ; 2
(push) ; 2
; [exec]
; _tmp1 := new(arrayContents)
(declare-const _tmp1@4 $Ref)
(assert (not (= _tmp1@4 $Ref.null)))
(declare-const arrayContents@5 $Seq<Int>)
(assert (and (not (= _tmp1@4 v@2)) (not (= _tmp1@4 res@1))))
; [exec]
; _tmp1.arrayContents := Seq(32)
; [eval] Seq(32)
(assert (= ($Seq.length ($Seq.singleton 32)) 1))
; [exec]
; v := _tmp1
; [exec]
; res := v
(pop) ; 2
(pop) ; 1
; ---------- SeqUtils_ArrayToSeq ----------
(declare-const array@6 $Ref)
(declare-const res@7 $Seq<Int>)
(declare-const result2@8 $Seq<Int>)
(declare-const ind@9 Int)
(push) ; 1
(assert (not (= array@6 $Ref.null)))
(declare-const $t@10 $Seq<Int>)
(push) ; 2
(declare-const $t@11 $Snap)
(declare-const $t@12 $Seq<Int>)
(assert (= $t@11 ($Snap.combine ($SortWrappers.$Seq<Int>To$Snap $t@12) $Snap.unit)))
; [eval] |res| == |array.arrayContents|
; [eval] |res|
; [eval] |array.arrayContents|
(assert (= ($Seq.length res@7) ($Seq.length $t@12)))
; [eval] (forall i: Int :: 0 <= i && i < |array.arrayContents| ==> array.arrayContents[i] == res[i])
(declare-const i@13 Int)
(push) ; 3
; [eval] 0 <= i && i < |array.arrayContents| ==> array.arrayContents[i] == res[i]
; [eval] 0 <= i && i < |array.arrayContents|
; [eval] 0 <= i
; [eval] 0 <= i ==> i < |array.arrayContents|
; [eval] 0 <= i
(push) ; 4
(set-option :timeout 250)
(push) ; 5
(assert (not (not (<= 0 i@13))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= 0 i@13)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 0] 0 <= i@13
(assert (<= 0 i@13))
; [eval] i < |array.arrayContents|
; [eval] |array.arrayContents|
(pop) ; 5
(push) ; 5
; [else-branch 0] !0 <= i@13
(assert (not (<= 0 i@13)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(push) ; 4
(push) ; 5
(assert (not (not (and (<= 0 i@13) (implies (<= 0 i@13) (< i@13 ($Seq.length $t@12)))))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (and (<= 0 i@13) (implies (<= 0 i@13) (< i@13 ($Seq.length $t@12))))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 1] 0 <= i@13 && 0 <= i@13 ==> i@13 < |$t@12|
(assert (and (<= 0 i@13) (implies (<= 0 i@13) (< i@13 ($Seq.length $t@12)))))
; [eval] array.arrayContents[i] == res[i]
; [eval] array.arrayContents[i]
; [eval] res[i]
(pop) ; 5
(push) ; 5
; [else-branch 1] !0 <= i@13 && 0 <= i@13 ==> i@13 < |$t@12|
(assert (not (and (<= 0 i@13) (implies (<= 0 i@13) (< i@13 ($Seq.length $t@12))))))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(pop) ; 3
; Nested auxiliary terms
(assert (and
  (forall ((i@13 Int)) (!
    (implies
      (and (<= 0 i@13) (implies (<= 0 i@13) (< i@13 ($Seq.length $t@12))))
      (= ($Seq.index $t@12 i@13) ($Seq.index res@7 i@13)))
    :pattern (($Seq.index $t@12 i@13))
    :qid |prog.l13|))
  (forall ((i@13 Int)) (!
    (implies
      (and (<= 0 i@13) (implies (<= 0 i@13) (< i@13 ($Seq.length $t@12))))
      (= ($Seq.index $t@12 i@13) ($Seq.index res@7 i@13)))
    :pattern (($Seq.index res@7 i@13))
    :qid |prog.l13|))))
(pop) ; 2
(push) ; 2
; [eval] |array.arrayContents| == 0
; [eval] |array.arrayContents|
(push) ; 3
(assert (not (not (= ($Seq.length $t@10) 0))))
(check-sat)
; unknown
(pop) ; 3
; 0,00s
; (get-info :all-statistics)
(push) ; 3
(assert (not (= ($Seq.length $t@10) 0)))
(check-sat)
; unknown
(pop) ; 3
; 0,00s
; (get-info :all-statistics)
(push) ; 3
; [then-branch 2] |$t@10| == 0
(assert (= ($Seq.length $t@10) 0))
; [exec]
; res := Seq[Int]()
; [eval] Seq[Int]()
; [exec]
; label end
; [eval] |res| == |array.arrayContents|
; [eval] |res|
; [eval] |array.arrayContents|
(set-option :timeout 0)
(push) ; 4
(assert (not (= ($Seq.length $Seq.empty<Int>) ($Seq.length $t@10))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (= ($Seq.length $Seq.empty<Int>) ($Seq.length $t@10)))
; [eval] (forall i: Int :: 0 <= i && i < |array.arrayContents| ==> array.arrayContents[i] == res[i])
(declare-const i@14 Int)
(push) ; 4
; [eval] 0 <= i && i < |array.arrayContents| ==> array.arrayContents[i] == res[i]
; [eval] 0 <= i && i < |array.arrayContents|
; [eval] 0 <= i
; [eval] 0 <= i ==> i < |array.arrayContents|
; [eval] 0 <= i
(push) ; 5
(set-option :timeout 250)
(push) ; 6
(assert (not (not (<= 0 i@14))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
(assert (not (<= 0 i@14)))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
; [then-branch 3] 0 <= i@14
(assert (<= 0 i@14))
; [eval] i < |array.arrayContents|
; [eval] |array.arrayContents|
(pop) ; 6
(push) ; 6
; [else-branch 3] !0 <= i@14
(assert (not (<= 0 i@14)))
(pop) ; 6
(pop) ; 5
; Joined path conditions
; Joined path conditions
(push) ; 5
(push) ; 6
(assert (not (not (and (<= 0 i@14) (implies (<= 0 i@14) (< i@14 ($Seq.length $t@10)))))))
(check-sat)
; unsat
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
; [dead then-branch 4] 0 <= i@14 && 0 <= i@14 ==> i@14 < |$t@10|
(push) ; 6
; [else-branch 4] !0 <= i@14 && 0 <= i@14 ==> i@14 < |$t@10|
(assert (not (and (<= 0 i@14) (implies (<= 0 i@14) (< i@14 ($Seq.length $t@10))))))
(pop) ; 6
(pop) ; 5
; Joined path conditions
; [eval] array.arrayContents[i]
; [eval] res[i]
(pop) ; 4
; Nested auxiliary terms
(pop) ; 3
(push) ; 3
; [else-branch 2] |$t@10| != 0
(assert (not (= ($Seq.length $t@10) 0)))
(pop) ; 3
; [eval] !(|array.arrayContents| == 0)
; [eval] |array.arrayContents| == 0
; [eval] |array.arrayContents|
(push) ; 3
(assert (not (= ($Seq.length $t@10) 0)))
(check-sat)
; unknown
(pop) ; 3
; 0,00s
; (get-info :all-statistics)
(push) ; 3
(assert (not (not (= ($Seq.length $t@10) 0))))
(check-sat)
; unknown
(pop) ; 3
; 0,00s
; (get-info :all-statistics)
(push) ; 3
; [then-branch 5] |$t@10| != 0
(assert (not (= ($Seq.length $t@10) 0)))
; [exec]
; result2 := Seq(array.arrayContents[0])
; [eval] Seq(array.arrayContents[0])
; [eval] array.arrayContents[0]
(assert (= ($Seq.length ($Seq.singleton ($Seq.index $t@10 0))) 1))
; [exec]
; ind := 1
; loop at tmp919D.tmp@23.2
(declare-const ind@15 Int)
(declare-const result2@16 $Seq<Int>)
(push) ; 4
; Loop: Check specs well-definedness
(declare-const $t@17 $Snap)
(declare-const $t@18 $Seq<Int>)
(assert (= $t@17 ($Snap.combine ($SortWrappers.$Seq<Int>To$Snap $t@18) $Snap.unit)))
; [eval] |result2| == ind
; [eval] |result2|
(assert (= ($Seq.length result2@16) ind@15))
; [eval] (forall i2: Int :: 0 <= i2 && i2 < ind ==> array.arrayContents[i2] == result2[i2])
(declare-const i2@19 Int)
(push) ; 5
; [eval] 0 <= i2 && i2 < ind ==> array.arrayContents[i2] == result2[i2]
; [eval] 0 <= i2 && i2 < ind
; [eval] 0 <= i2
; [eval] 0 <= i2 ==> i2 < ind
; [eval] 0 <= i2
(push) ; 6
(push) ; 7
(assert (not (not (<= 0 i2@19))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (<= 0 i2@19)))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 6] 0 <= i2@19
(assert (<= 0 i2@19))
; [eval] i2 < ind
(pop) ; 7
(push) ; 7
; [else-branch 6] !0 <= i2@19
(assert (not (<= 0 i2@19)))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(push) ; 6
(push) ; 7
(assert (not (not (and (<= 0 i2@19) (implies (<= 0 i2@19) (< i2@19 ind@15))))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (and (<= 0 i2@19) (implies (<= 0 i2@19) (< i2@19 ind@15)))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 7] 0 <= i2@19 && 0 <= i2@19 ==> i2@19 < ind@15
(assert (and (<= 0 i2@19) (implies (<= 0 i2@19) (< i2@19 ind@15))))
; [eval] array.arrayContents[i2] == result2[i2]
; [eval] array.arrayContents[i2]
; [eval] result2[i2]
(pop) ; 7
(push) ; 7
; [else-branch 7] !0 <= i2@19 && 0 <= i2@19 ==> i2@19 < ind@15
(assert (not (and (<= 0 i2@19) (implies (<= 0 i2@19) (< i2@19 ind@15)))))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(pop) ; 5
; Nested auxiliary terms
(assert (and
  (forall ((i2@19 Int)) (!
    (implies
      (and (<= 0 i2@19) (implies (<= 0 i2@19) (< i2@19 ind@15)))
      (= ($Seq.index $t@18 i2@19) ($Seq.index result2@16 i2@19)))
    :pattern (($Seq.index $t@18 i2@19))
    :qid |prog.l26|))
  (forall ((i2@19 Int)) (!
    (implies
      (and (<= 0 i2@19) (implies (<= 0 i2@19) (< i2@19 ind@15)))
      (= ($Seq.index $t@18 i2@19) ($Seq.index result2@16 i2@19)))
    :pattern (($Seq.index result2@16 i2@19))
    :qid |prog.l26|))))
(declare-const $t@20 $Snap)
(assert (= $t@20 $Snap.unit))
; [eval] ind != |array.arrayContents|
; [eval] |array.arrayContents|
(assert (not (= ind@15 ($Seq.length $t@18))))
(check-sat)
; unknown
(pop) ; 4
(push) ; 4
; Loop: Establish loop invariant
; [eval] |result2| == ind
; [eval] |result2|
; [eval] (forall i2: Int :: 0 <= i2 && i2 < ind ==> array.arrayContents[i2] == result2[i2])
(declare-const i2@21 Int)
(push) ; 5
; [eval] 0 <= i2 && i2 < ind ==> array.arrayContents[i2] == result2[i2]
; [eval] 0 <= i2 && i2 < ind
; [eval] 0 <= i2
; [eval] 0 <= i2 ==> i2 < ind
; [eval] 0 <= i2
(push) ; 6
(push) ; 7
(assert (not (not (<= 0 i2@21))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (<= 0 i2@21)))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 8] 0 <= i2@21
(assert (<= 0 i2@21))
; [eval] i2 < ind
(pop) ; 7
(push) ; 7
; [else-branch 8] !0 <= i2@21
(assert (not (<= 0 i2@21)))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(push) ; 6
(push) ; 7
(assert (not (not (and (<= 0 i2@21) (implies (<= 0 i2@21) (< i2@21 1))))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (and (<= 0 i2@21) (implies (<= 0 i2@21) (< i2@21 1)))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 9] 0 <= i2@21 && 0 <= i2@21 ==> i2@21 < 1
(assert (and (<= 0 i2@21) (implies (<= 0 i2@21) (< i2@21 1))))
; [eval] array.arrayContents[i2] == result2[i2]
; [eval] array.arrayContents[i2]
; [eval] result2[i2]
(pop) ; 7
(push) ; 7
; [else-branch 9] !0 <= i2@21 && 0 <= i2@21 ==> i2@21 < 1
(assert (not (and (<= 0 i2@21) (implies (<= 0 i2@21) (< i2@21 1)))))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(pop) ; 5
; Nested auxiliary terms
(set-option :timeout 0)
(push) ; 5
(assert (not (and
  (forall ((i2@21 Int)) (!
    (implies
      (and (<= 0 i2@21) (implies (<= 0 i2@21) (< i2@21 1)))
      (=
        ($Seq.index $t@10 i2@21)
        ($Seq.index ($Seq.singleton ($Seq.index $t@10 0)) i2@21)))
    :pattern (($Seq.index $t@10 i2@21))
    :qid |prog.l26|))
  (forall ((i2@21 Int)) (!
    (implies
      (and (<= 0 i2@21) (implies (<= 0 i2@21) (< i2@21 1)))
      (=
        ($Seq.index $t@10 i2@21)
        ($Seq.index ($Seq.singleton ($Seq.index $t@10 0)) i2@21)))
    :pattern (($Seq.index ($Seq.singleton ($Seq.index $t@10 0)) i2@21))
    :qid |prog.l26|)))))
(check-sat)
; unsat
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(assert (and
  (forall ((i2@21 Int)) (!
    (implies
      (and (<= 0 i2@21) (implies (<= 0 i2@21) (< i2@21 1)))
      (=
        ($Seq.index $t@10 i2@21)
        ($Seq.index ($Seq.singleton ($Seq.index $t@10 0)) i2@21)))
    :pattern (($Seq.index $t@10 i2@21))
    :qid |prog.l26|))
  (forall ((i2@21 Int)) (!
    (implies
      (and (<= 0 i2@21) (implies (<= 0 i2@21) (< i2@21 1)))
      (=
        ($Seq.index $t@10 i2@21)
        ($Seq.index ($Seq.singleton ($Seq.index $t@10 0)) i2@21)))
    :pattern (($Seq.index ($Seq.singleton ($Seq.index $t@10 0)) i2@21))
    :qid |prog.l26|))))
(pop) ; 4
; Loop: Verify loop body
(push) ; 4
(assert (not (= ind@15 ($Seq.length $t@18))))
(assert (= $t@20 $Snap.unit))
(assert (and
  (forall ((i2@19 Int)) (!
    (implies
      (and (<= 0 i2@19) (implies (<= 0 i2@19) (< i2@19 ind@15)))
      (= ($Seq.index $t@18 i2@19) ($Seq.index result2@16 i2@19)))
    :pattern (($Seq.index $t@18 i2@19))
    :qid |prog.l26|))
  (forall ((i2@19 Int)) (!
    (implies
      (and (<= 0 i2@19) (implies (<= 0 i2@19) (< i2@19 ind@15)))
      (= ($Seq.index $t@18 i2@19) ($Seq.index result2@16 i2@19)))
    :pattern (($Seq.index result2@16 i2@19))
    :qid |prog.l26|))))
(assert (= ($Seq.length result2@16) ind@15))
(assert (= $t@17 ($Snap.combine ($SortWrappers.$Seq<Int>To$Snap $t@18) $Snap.unit)))
; [exec]
; result2 := result2 ++ Seq(array.arrayContents[ind])
; [eval] result2 ++ Seq(array.arrayContents[ind])
; [eval] Seq(array.arrayContents[ind])
; [eval] array.arrayContents[ind]
(assert (= ($Seq.length ($Seq.singleton ($Seq.index $t@18 ind@15))) 1))
; [exec]
; ind := ind + 1
; [eval] ind + 1
(declare-const ind@22 Int)
(assert (= ind@22 (+ ind@15 1)))
; [eval] |result2| == ind
; [eval] |result2|
(push) ; 5
(assert (not (=
  ($Seq.length
    ($Seq.append result2@16 ($Seq.singleton ($Seq.index $t@18 ind@15))))
  ind@22)))
(check-sat)
; unsat
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(assert (=
  ($Seq.length
    ($Seq.append result2@16 ($Seq.singleton ($Seq.index $t@18 ind@15))))
  ind@22))
; [eval] (forall i2: Int :: 0 <= i2 && i2 < ind ==> array.arrayContents[i2] == result2[i2])
(declare-const i2@23 Int)
(push) ; 5
; [eval] 0 <= i2 && i2 < ind ==> array.arrayContents[i2] == result2[i2]
; [eval] 0 <= i2 && i2 < ind
; [eval] 0 <= i2
; [eval] 0 <= i2 ==> i2 < ind
; [eval] 0 <= i2
(push) ; 6
(set-option :timeout 250)
(push) ; 7
(assert (not (not (<= 0 i2@23))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (<= 0 i2@23)))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 10] 0 <= i2@23
(assert (<= 0 i2@23))
; [eval] i2 < ind
(pop) ; 7
(push) ; 7
; [else-branch 10] !0 <= i2@23
(assert (not (<= 0 i2@23)))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(push) ; 6
(push) ; 7
(assert (not (not (and (<= 0 i2@23) (implies (<= 0 i2@23) (< i2@23 ind@22))))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (and (<= 0 i2@23) (implies (<= 0 i2@23) (< i2@23 ind@22)))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 11] 0 <= i2@23 && 0 <= i2@23 ==> i2@23 < ind@22
(assert (and (<= 0 i2@23) (implies (<= 0 i2@23) (< i2@23 ind@22))))
; [eval] array.arrayContents[i2] == result2[i2]
; [eval] array.arrayContents[i2]
; [eval] result2[i2]
(pop) ; 7
(push) ; 7
; [else-branch 11] !0 <= i2@23 && 0 <= i2@23 ==> i2@23 < ind@22
(assert (not (and (<= 0 i2@23) (implies (<= 0 i2@23) (< i2@23 ind@22)))))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(pop) ; 5
; Nested auxiliary terms
(set-option :timeout 0)
(push) ; 5
(assert (not (and
  (forall ((i2@23 Int)) (!
    (implies
      (and (<= 0 i2@23) (implies (<= 0 i2@23) (< i2@23 ind@22)))
      (=
        ($Seq.index $t@18 i2@23)
        ($Seq.index
          ($Seq.append result2@16 ($Seq.singleton ($Seq.index $t@18 ind@15)))
          i2@23)))
    :pattern (($Seq.index $t@18 i2@23))
    :qid |prog.l26|))
  (forall ((i2@23 Int)) (!
    (implies
      (and (<= 0 i2@23) (implies (<= 0 i2@23) (< i2@23 ind@22)))
      (=
        ($Seq.index $t@18 i2@23)
        ($Seq.index
          ($Seq.append result2@16 ($Seq.singleton ($Seq.index $t@18 ind@15)))
          i2@23)))
    :pattern (($Seq.index
      ($Seq.append result2@16 ($Seq.singleton ($Seq.index $t@18 ind@15)))
      i2@23))
    :qid |prog.l26|)))))
(check-sat)
; unsat
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(assert (and
  (forall ((i2@23 Int)) (!
    (implies
      (and (<= 0 i2@23) (implies (<= 0 i2@23) (< i2@23 ind@22)))
      (=
        ($Seq.index $t@18 i2@23)
        ($Seq.index
          ($Seq.append result2@16 ($Seq.singleton ($Seq.index $t@18 ind@15)))
          i2@23)))
    :pattern (($Seq.index $t@18 i2@23))
    :qid |prog.l26|))
  (forall ((i2@23 Int)) (!
    (implies
      (and (<= 0 i2@23) (implies (<= 0 i2@23) (< i2@23 ind@22)))
      (=
        ($Seq.index $t@18 i2@23)
        ($Seq.index
          ($Seq.append result2@16 ($Seq.singleton ($Seq.index $t@18 ind@15)))
          i2@23)))
    :pattern (($Seq.index
      ($Seq.append result2@16 ($Seq.singleton ($Seq.index $t@18 ind@15)))
      i2@23))
    :qid |prog.l26|))))
(pop) ; 4
; Loop: Continue after loop
(push) ; 4
(assert (and
  (forall ((i2@21 Int)) (!
    (implies
      (and (<= 0 i2@21) (implies (<= 0 i2@21) (< i2@21 1)))
      (=
        ($Seq.index $t@10 i2@21)
        ($Seq.index ($Seq.singleton ($Seq.index $t@10 0)) i2@21)))
    :pattern (($Seq.index $t@10 i2@21))
    :qid |prog.l26|))
  (forall ((i2@21 Int)) (!
    (implies
      (and (<= 0 i2@21) (implies (<= 0 i2@21) (< i2@21 1)))
      (=
        ($Seq.index $t@10 i2@21)
        ($Seq.index ($Seq.singleton ($Seq.index $t@10 0)) i2@21)))
    :pattern (($Seq.index ($Seq.singleton ($Seq.index $t@10 0)) i2@21))
    :qid |prog.l26|))))
(declare-const $t@24 $Snap)
(declare-const $t@25 $Seq<Int>)
(assert (= $t@24 ($Snap.combine ($SortWrappers.$Seq<Int>To$Snap $t@25) $Snap.unit)))
; [eval] |result2| == ind
; [eval] |result2|
(assert (= ($Seq.length result2@16) ind@15))
; [eval] (forall i2: Int :: 0 <= i2 && i2 < ind ==> array.arrayContents[i2] == result2[i2])
(declare-const i2@26 Int)
(push) ; 5
; [eval] 0 <= i2 && i2 < ind ==> array.arrayContents[i2] == result2[i2]
; [eval] 0 <= i2 && i2 < ind
; [eval] 0 <= i2
; [eval] 0 <= i2 ==> i2 < ind
; [eval] 0 <= i2
(push) ; 6
(set-option :timeout 250)
(push) ; 7
(assert (not (not (<= 0 i2@26))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (<= 0 i2@26)))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 12] 0 <= i2@26
(assert (<= 0 i2@26))
; [eval] i2 < ind
(pop) ; 7
(push) ; 7
; [else-branch 12] !0 <= i2@26
(assert (not (<= 0 i2@26)))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(push) ; 6
(push) ; 7
(assert (not (not (and (<= 0 i2@26) (implies (<= 0 i2@26) (< i2@26 ind@15))))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (and (<= 0 i2@26) (implies (<= 0 i2@26) (< i2@26 ind@15)))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 13] 0 <= i2@26 && 0 <= i2@26 ==> i2@26 < ind@15
(assert (and (<= 0 i2@26) (implies (<= 0 i2@26) (< i2@26 ind@15))))
; [eval] array.arrayContents[i2] == result2[i2]
; [eval] array.arrayContents[i2]
; [eval] result2[i2]
(pop) ; 7
(push) ; 7
; [else-branch 13] !0 <= i2@26 && 0 <= i2@26 ==> i2@26 < ind@15
(assert (not (and (<= 0 i2@26) (implies (<= 0 i2@26) (< i2@26 ind@15)))))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(pop) ; 5
; Nested auxiliary terms
(assert (and
  (forall ((i2@26 Int)) (!
    (implies
      (and (<= 0 i2@26) (implies (<= 0 i2@26) (< i2@26 ind@15)))
      (= ($Seq.index $t@25 i2@26) ($Seq.index result2@16 i2@26)))
    :pattern (($Seq.index $t@25 i2@26))
    :qid |prog.l26|))
  (forall ((i2@26 Int)) (!
    (implies
      (and (<= 0 i2@26) (implies (<= 0 i2@26) (< i2@26 ind@15)))
      (= ($Seq.index $t@25 i2@26) ($Seq.index result2@16 i2@26)))
    :pattern (($Seq.index result2@16 i2@26))
    :qid |prog.l26|))))
; [eval] !(ind != |array.arrayContents|)
; [eval] ind != |array.arrayContents|
; [eval] |array.arrayContents|
(assert (= ind@15 ($Seq.length $t@25)))
(check-sat)
; unknown
; [exec]
; res := result2
; [exec]
; label end
; [eval] |res| == |array.arrayContents|
; [eval] |res|
; [eval] |array.arrayContents|
(set-option :timeout 0)
(push) ; 5
(assert (not (= ($Seq.length result2@16) ($Seq.length $t@25))))
(check-sat)
; unsat
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(assert (= ($Seq.length result2@16) ($Seq.length $t@25)))
; [eval] (forall i: Int :: 0 <= i && i < |array.arrayContents| ==> array.arrayContents[i] == res[i])
(declare-const i@27 Int)
(push) ; 5
; [eval] 0 <= i && i < |array.arrayContents| ==> array.arrayContents[i] == res[i]
; [eval] 0 <= i && i < |array.arrayContents|
; [eval] 0 <= i
; [eval] 0 <= i ==> i < |array.arrayContents|
; [eval] 0 <= i
(push) ; 6
(set-option :timeout 250)
(push) ; 7
(assert (not (not (<= 0 i@27))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (<= 0 i@27)))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 14] 0 <= i@27
(assert (<= 0 i@27))
; [eval] i < |array.arrayContents|
; [eval] |array.arrayContents|
(pop) ; 7
(push) ; 7
; [else-branch 14] !0 <= i@27
(assert (not (<= 0 i@27)))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(push) ; 6
(push) ; 7
(assert (not (not (and (<= 0 i@27) (implies (<= 0 i@27) (< i@27 ($Seq.length $t@25)))))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (and (<= 0 i@27) (implies (<= 0 i@27) (< i@27 ($Seq.length $t@25))))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 15] 0 <= i@27 && 0 <= i@27 ==> i@27 < |$t@25|
(assert (and (<= 0 i@27) (implies (<= 0 i@27) (< i@27 ($Seq.length $t@25)))))
; [eval] array.arrayContents[i] == res[i]
; [eval] array.arrayContents[i]
; [eval] res[i]
(pop) ; 7
(push) ; 7
; [else-branch 15] !0 <= i@27 && 0 <= i@27 ==> i@27 < |$t@25|
(assert (not (and (<= 0 i@27) (implies (<= 0 i@27) (< i@27 ($Seq.length $t@25))))))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(pop) ; 5
; Nested auxiliary terms
(set-option :timeout 0)
(push) ; 5
(assert (not (and
  (forall ((i@27 Int)) (!
    (implies
      (and (<= 0 i@27) (implies (<= 0 i@27) (< i@27 ($Seq.length $t@25))))
      (= ($Seq.index $t@25 i@27) ($Seq.index result2@16 i@27)))
    :pattern (($Seq.index $t@25 i@27))
    :qid |prog.l13|))
  (forall ((i@27 Int)) (!
    (implies
      (and (<= 0 i@27) (implies (<= 0 i@27) (< i@27 ($Seq.length $t@25))))
      (= ($Seq.index $t@25 i@27) ($Seq.index result2@16 i@27)))
    :pattern (($Seq.index result2@16 i@27))
    :qid |prog.l13|)))))
(check-sat)
; unsat
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(assert (and
  (forall ((i@27 Int)) (!
    (implies
      (and (<= 0 i@27) (implies (<= 0 i@27) (< i@27 ($Seq.length $t@25))))
      (= ($Seq.index $t@25 i@27) ($Seq.index result2@16 i@27)))
    :pattern (($Seq.index $t@25 i@27))
    :qid |prog.l13|))
  (forall ((i@27 Int)) (!
    (implies
      (and (<= 0 i@27) (implies (<= 0 i@27) (< i@27 ($Seq.length $t@25))))
      (= ($Seq.index $t@25 i@27) ($Seq.index result2@16 i@27)))
    :pattern (($Seq.index result2@16 i@27))
    :qid |prog.l13|))))
(pop) ; 4
(pop) ; 3
(push) ; 3
; [else-branch 5] |$t@10| == 0
(assert (= ($Seq.length $t@10) 0))
(pop) ; 3
(pop) ; 2
(pop) ; 1
