use std::fs;

fn main() {
    let constants = fs::read_to_string("src/utils/constants.rs")
        .expect("Missing src/utils/constants.rs. Copy src/utils/constants.example.rs to constants.rs and fill it out.");

    if constants.contains("<REPLACE_ME>") {
        panic!(
            r#"
            ========================================
            src/utils/constants.rs is not configured.
            
            Copy src/utils/constants.example.rs to src/utils/constants.rs
            and replace all '<REPLACE_ME>' values.
            ========================================\n"#
        )
    }
}
